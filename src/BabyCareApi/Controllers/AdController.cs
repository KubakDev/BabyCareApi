using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using BabyCareApi.Models.Common;
using BabyCareApi.Models.Requests;
using BabyCareApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BabyCareApi.Controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("v1/ads")]
public class AdController : ControllerBase
{

  private readonly AdService _AdService;

  public AdController(AdService adService)
  {

    _AdService = adService;
  }


  [HttpGet]
  public async Task<List<Ad>> ListAds([FromQuery] ListAds model)
  {
    return await _AdService.ListAsync(model);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Ad>> GetAdById([IsBsonId] string id)
  {
    var ad = await _AdService.GetByIdAsync(id);

    if (ad is null)
      return NotFound();

    return Ok(ad);

  }

  [HttpPost]
  public async Task<ActionResult<Ad>> CreateAd(CreateAdRequest model)
  {

    var now = DateTime.UtcNow;
    var ad = model.ToAd(model.Reach);


    await _AdService.CreateAsync(ad);

    return CreatedAtAction(nameof(CreateAd), ad);

  }

  [HttpPut("{id}")]
  public async Task<ActionResult<Ad>> UpdateAd([IsBsonId] string id, UpdateAdRequest model)
  {
    var ad = await _AdService.GetByIdAsync(id);
    if (ad is null)
      return NotFound();

    var updatedAd = await _AdService.UpdateAsync(id, model);

    return Ok(updatedAd);
  }

  [HttpPut("increment-reach/{id}")]
  public async Task<ActionResult<Ad>> IncrementReach(string id)
  {
    var ad = await _AdService.GetByIdAsync(id);
    if (ad is null)
      return NotFound();


    await _AdService.IncrementReachAsync(id);

    return CreatedAtAction(nameof(IncrementReach), ad);

  }
}