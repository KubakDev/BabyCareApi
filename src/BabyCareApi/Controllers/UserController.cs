using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using BabyCareApi.Models.Auth;
using BabyCareApi.Models.Requests;
using BabyCareApi.Services;
using BabyCareteApi.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BabyCareApi.Extensions;
using BabyCareApi.Models.Common;

namespace BabyCareApi.Controllers;
// [Authorize(Roles = Roles.Admin)]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[Route("v1/users")]
public class UsersController : ControllerBase
{

  private readonly UserService _UserService;

  public UsersController(UserService userService)
  {
    _UserService = userService;
  }

  [HttpGet]
  public Task<List<User>> ListUsers([FromQuery] ListUsers model)
  {
    return _UserService.ListAsync(model);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<User>> GetUserById([IsBsonId] string id)
  {
    var user = await _UserService.GetByIdAsync(id);

    if (user is null)
      return NotFound();

    return Ok(user);
  }

  [HttpPost]
  public async Task<ActionResult<User>> CreateUser(CreateUser model)
  {
    model.Username = model.Username.ToLowerInvariant();

    if (await _UserService.UsernameExistsAsync(model.Username))
      return BadRequest();

    var now = DateTime.UtcNow;
    var user = model.ToUser(now);

    await _UserService.CreateAsync(user);

    return CreatedAtAction(nameof(CreateUser), user);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<User>> UpdateUser([IsBsonId] string id, UpdateUser model)
  {
    var user = await _UserService.GetByIdAsync(id);
    if (user is null)
      return NotFound();

    if (model.Password != null && user.Role == Role.User && User.GetRole() != Role.User) return BadRequest("Cannot change user's password");

    // Do not allow changing status of self
    if (model.Status != null && id == User.GetId())
      model.Status = null;

    var updatedUser = await _UserService.UpdateAsync(id, model);

    if (updatedUser is null)
      return NotFound();

    return Ok(updatedUser);
  }
}
