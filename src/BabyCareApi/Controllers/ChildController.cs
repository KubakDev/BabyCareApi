using System.ComponentModel.DataAnnotations;
using BabyCareApi.Models.Common;
using BabyCareApi.Models.Requests;
using BabyCareApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BabyCareApi.Controllers;
[Authorize]
[ApiController]
[Route("v1/chilren")]

public class ChildController : ControllerBase
{
  private readonly ChildService _ChildService;

  public ChildController(ChildService childService)
  {

    _ChildService = childService;

  }


  [HttpGet]
  public async Task<List<Child>> ListChildren([FromQuery] ListChildren model) => await _ChildService.ListAsync(model);

  [HttpGet("{id}")]
  public async Task<ActionResult<Child>> GetChildById([IsBsonId] string id)
  {
    var child = await _ChildService.GetByIdAsync(id);

    if (child is null)
      return NotFound();

    return Ok(child);

  }

  [HttpPost]
  public async Task<ActionResult<Child>> CreateChild(CreateChildRequest model)
  {
    var now = DateTime.UtcNow;
    var child = model.ToChild(now);

    await _ChildService.CreateAsync(child);

    return CreatedAtAction(nameof(CreateChild), child);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<Child>> UpdateChild([IsBsonId] string id, UpdateChildRequest model)
  {
    var child = await _ChildService.GetByIdAsync(id);

    if (child is null)
      return NotFound();

    var updatedChild = await _ChildService.UpdateAsync(id, model);

    if (updatedChild is null)
      return NotFound();

    return Ok(updatedChild);


  }

}