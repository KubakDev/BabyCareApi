using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BabyCareApi.Extensions;
using BabyCareApi.Models;
using BabyCareApi.Models.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BabyCareApi.Services;
public class TokenService
{

  private readonly AuthOptions _AuthOptions;
  private readonly TokenValidationParameters _ExpiredTokenValidation;

  public TokenService(IOptions<AuthOptions> authOptions)
  {
    _AuthOptions = authOptions.Value;

    _ExpiredTokenValidation = new TokenValidationParameters
    {
      ClockSkew = TimeSpan.Zero,
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = false, // here we are saying that we don't care about the token's expiration date
      ValidateIssuerSigningKey = true,

      ValidIssuer = _AuthOptions.Issuer,
      ValidAudiences = new[]
           {
                        Audience.UserApp.ToString(),
                        Audience.AdminWebPanel.ToString(),
            },
      IssuerSigningKey = new SymmetricSecurityKey(_AuthOptions.SecretBytes),
    };
  }

  public record GeneratedToken(string Id, string Token, DateTime ExpiresAt);

  public GeneratedToken GenerateToken(in User user, DateTime? expiresAt = null)
  {
    var claims = new List<Claim>
            {
                new Claim("sub", user.Id),
                new(ClaimTypes.Role, user.Role.ToString())
            };

    return GenerateToken(user.Role.GetAudience(), claims, expiresAt);
  }

  public GeneratedToken GenerateToken(in Audience audience, List<Claim> claims, DateTime? expiresAt = null)
  {
    var tokenId = GenerateNanoId();
    var expires = expiresAt ?? DateTime.UtcNow + _AuthOptions.TokenExpirationTime;

    claims.Add(new("jti", tokenId));
    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Issuer = _AuthOptions.Issuer,
      Audience = audience.ToString(),
      Subject = new(claims),
      Expires = expires,
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_AuthOptions.SecretBytes), SecurityAlgorithms.HmacSha256Signature)
    };

    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
    var token = tokenHandler.WriteToken(securityToken);

    return new(tokenId, token, expires);
  }

  public string GenerateInvoiceToken(in long orderId)
  {
    var claims = new List<Claim>
            {
                new Claim("order_id", orderId.ToString())
            };

    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Issuer = _AuthOptions.Issuer,
      Subject = new(claims),
      Expires = DateTime.UtcNow + TimeSpan.FromDays(7),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_AuthOptions.SecretBytes), SecurityAlgorithms.HmacSha256Signature)
    };

    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
    var token = tokenHandler.WriteToken(securityToken);

    return token;
  }

  public ClaimsPrincipal GetUserFromToken(string token)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var principal = tokenHandler.ValidateToken(token, _ExpiredTokenValidation, out var securityToken);

    if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
      throw new SecurityTokenException("Invalid Token");

    return principal;
  }

  public ClaimsPrincipal DecodeInvoiceToken(in string token)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var validationParams = new TokenValidationParameters
    {
      ValidateAudience = false,
      ValidateIssuerSigningKey = true,

      ValidIssuer = _AuthOptions.Issuer,
      IssuerSigningKey = new SymmetricSecurityKey(_AuthOptions.SecretBytes),
    };
    var principal = tokenHandler.ValidateToken(token, validationParams, out var securityToken);

    if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
      throw new SecurityTokenException("Invalid Token");

    return principal;
  }

  private static string GenerateNanoId() => Nanoid.Nanoid.Generate("123456789ABCDEFGHIJKLMNPQRSTUVWXYZ");
}