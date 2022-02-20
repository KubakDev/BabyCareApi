using System.ComponentModel.DataAnnotations;
using BabyCareApi.Extensions;
using BabyCareApi.Models.Common;
using BabyCareApi.Models.Requests;
using BabyCareApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using BabyCareApi.Models.Auth;

namespace BabyCareApi.Controllers;

[ApiController]
[Authorize(Roles = Roles.Admin)]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[Route("v1/advices")]
public class AdviceController : ControllerBase
{

  public AdviceService _AdviceService { get; set; }
  public UserService _UserService { get; set; }
  public AdviceController(AdviceService adviceService, UserService userService)
  {
    _AdviceService = adviceService;
    _UserService = userService;
  }


  [HttpGet]
  public Task<List<Advice>> ListAdvices([FromQuery] ListAdvices model)
  {
    var user = _UserService.GetByIdAsync(User.GetId());

    // model.Tags = user.dise

    return _AdviceService.ListAsync(model);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Advice>> GetAdviceById([IsBsonId] string id)
  {
    var advice = await _AdviceService.GetByIdAsync(id);

    if (advice is null)
      return NotFound();

    return Ok(advice);
  }

  [HttpPost]
  public async Task<ActionResult<Advice>> CreateAdvice(CreateAdviceRequest model)
  {
    var now = DateTime.UtcNow;

    var advice = model.ToAdvice(now);

    await _AdviceService.CreateAsync(advice);

    return CreatedAtAction(nameof(CreateAdvice), advice);

  }

  [HttpPut("{id}")]
  public async Task<ActionResult<Advice>> UpdateAdvice([IsBsonId] string id, CreateAdviceRequest model)
  {
    var advice = await _AdviceService.GetByIdAsync(id);

    if (advice is null)
      return NotFound();

    var updatedAdvice = await _AdviceService.UpdateAsync(id, model);

    return Ok(updatedAdvice);
  }
}