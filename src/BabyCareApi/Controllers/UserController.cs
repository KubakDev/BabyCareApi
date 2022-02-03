using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using BabyCareApi.Models;
using BabyCareApi.Models.Auth;
using BabyCareApi.Models.Requests;
using BabyCareApi.Services;
using BabyCareteApi.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BabyCareApi.Extensions;

namespace BabyCareApi.Controllers;

[Authorize(Roles = Roles.Admin)]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("v1/users")]

public class UserController : ControllerBase
{
  private readonly UserService _UserService;
  public UserController(UserService userService)
  {
    _UserService = userService;

  }

  [HttpGet]
  public async Task<List<User>> ListUsers([FromQuery] ListUsers model) => await _UserService.ListAsync(model);


  [HttpGet("{id}")]
  public async Task<ActionResult<User>> GetUserById([IsBsonId] string id)
  {
    var user = await _UserService.GetByIdAsync(id);

    if (user is null) return NotFound();

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

    model.Username = model.Username?.ToLowerInvariant();

    if (model.Username != null && model.Username != user.Username && await _UserService.UsernameExistsAsync(model.Username))
      return BadRequest();


    // Do not allow changing status of self
    if (model.Status != null && id == User.GetId())
      model.Status = null;

    var updatedUser = await _UserService.UpdateAsync(id, model);

    if (updatedUser is null)
      return NotFound();

    return Ok(updatedUser);
  }
}
