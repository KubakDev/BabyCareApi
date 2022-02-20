using System.Net.Mime;
using BabyCareApi.Extensions;
using BabyCareApi.Models;
using BabyCareApi.Models.Auth;
using BabyCareApi.Models.Common;
using BabyCareApi.Models.Requests;
using BabyCareApi.Models.Responses;
using BabyCareApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BabyCareApi.Services.TokenService;

namespace BabyCareApi.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[Route("v1/auth")]

public class AuthController : ControllerBase
{
  private readonly UserService _UserService;
  private readonly TokenService _TokenService;
  private readonly RefreshTokenService _RefreshTokenService;

  public AuthController(
    UserService UserService,
    TokenService TokenService,
    RefreshTokenService RefreshTokenService
  )
  {

    _UserService = UserService;
    _TokenService = TokenService;
    _RefreshTokenService = RefreshTokenService;


  }

  // <summary>
  // Gets the current signed in user
  // </summary>


  [HttpGet("self")]
  public Task<User> GetSelf() => _UserService.GetByIdAsync(User.GetId()) as Task<User>;


  // TODO: Implement register

  [HttpPost("login")]
  public async Task<ActionResult<LoginResult>> Login(LoginRequest model)
  {
    model.Username = model.Username.ToLowerInvariant();

    var now = DateTime.UtcNow;
    var user = await _UserService.LoginAsync(model.Username, model.Password, model.Audience);

    if (user is null)
      return Unauthorized("AuthenticationFailed");

    if (user.Status is not UserStatus.Active)
      return Problem(type: "UserDeactivated", detail: $"User '{user.DisplayName}' is not active", statusCode: 403);

    var (accessToken, refreshToken) = await GenerateTokensAsync(user, model.Audience, now);

    await _RefreshTokenService.CreateAsync(refreshToken);

    var result = new LoginResult
    {
      User = user,
      Auth = new()
      {
        Access = new(accessToken.Token, accessToken.ExpiresAt),
        Refresh = new(refreshToken.Token, refreshToken.ExpiresAt)
      }
    };

    return Ok(result);
  }


  /// <summary>
  /// Logout from using a refresh token.
  /// </summary>
  [Authorize]
  [HttpPost("logout")]
  public async Task<ActionResult> Logout(LogoutRequest model)
  {
    var refreshToken = await _RefreshTokenService.DeleteByTokenAsync(model.RefreshToken, User.GetId());

    if (refreshToken is null)
      return Unauthorized();

    return Ok();
  }

  /// <summary>
  /// Use a refresh token to get a new access token.
  /// </summary>
  [HttpPost("refresh-token")]
  public async Task<ActionResult<AuthTokenResult>> RefreshToken(RefreshTokenRequest model)
  {
    string? jti;

    try
    {
      jti = _TokenService.GetUserFromToken(model.AccessToken).GetJti();
    }
    catch (Exception e)
    {
      ModelState.AddModelError(nameof(model.AccessToken), e.Message);
      return ValidationProblem();
    }

    var now = DateTime.UtcNow;
    var storedToken = await _RefreshTokenService.GetByTokenAsync(model.RefreshToken);

    if (storedToken == null || storedToken.JwtId != jti)
      return await InvalidRefreshToken();


    var user = await _UserService.GetByIdAsync(storedToken.UserId);

    if (user == null || user.Status is not UserStatus.Active)
      return await InvalidRefreshToken();

    var accessToken = _TokenService.GenerateToken(user);

    storedToken.JwtId = accessToken.Id;
    storedToken.ExpiresAt = now + TimeSpan.FromDays(90);
    storedToken.LastRefreshedAt = now;

    await _RefreshTokenService.UpdateAsync(storedToken);

    var result = new AuthTokenResult
    {
      Access = new(accessToken.Token, accessToken.ExpiresAt),
      Refresh = new(storedToken.Token, storedToken.ExpiresAt)
    };

    return Ok(result);

    async Task<UnauthorizedResult> InvalidRefreshToken()
    {
      Response.Headers.Append("Refresh-Token", "Invalid");

      if (storedToken != null)
        await _RefreshTokenService.DeleteByIdAsync(storedToken.Id);

      return Unauthorized();
    }
  }

  private async Task<(GeneratedToken, RefreshToken)> GenerateTokensAsync(User user, Audience audience, DateTime now)
  {
    var accessToken = _TokenService.GenerateToken(user);
    var refreshToken = new RefreshToken
    {
      JwtId = accessToken.Id,
      Token = await Nanoid.Nanoid.GenerateAsync("123456789ABCDEFGHIJKLMNPQRSTUVWXYZ"),
      UserId = user.Id,
      Audience = audience,
      ExpiresAt = now + TimeSpan.FromDays(90),
      CreatedAt = now,
      LastRefreshedAt = now
    };

    return (accessToken, refreshToken);
  }
}