using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using BabyCareApi.Models.Auth;
using BabyCareApi.Models.Common;
using BabyCareApi.Models.Requests;
using BabyCareApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabyCareApi.Controllers;

[ApiController]
[Authorize(Roles = Roles.Admin)]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[Route("v1/surveys")]

public class SurveyController : ControllerBase
{
  private readonly SurveyService _SurveyService;

  public SurveyController(SurveyService surveyService)
  {
    _SurveyService = surveyService;
  }

  [HttpGet]
  public async Task<List<Survey>> ListSurveys([FromQuery] ListSurveys model) => await _SurveyService.ListAsync(model);

  [HttpGet("{id}")]
  public async Task<ActionResult<Survey>> GetSurveyById([IsBsonId] string id)
  {

    var survey = await _SurveyService.GetByIdAsync(id);

    if (survey is null)
      return NotFound();

    return Ok(survey);

  }

  [HttpPost]
  public async Task<ActionResult<Survey>> CreateSurvey(CreateSurveyRequest model)
  {
    var now = DateTime.UtcNow;

    var survey = model.ToSurvey(now);

    await _SurveyService.CreateAsync(survey);

    return CreatedAtAction(nameof(CreateSurvey), survey);

  }

  [HttpPut("{id}")]
  public async Task<ActionResult<Survey>> UpdateSurvey([IsBsonId] string id, UpdateSurveyRequest model)
  {
    var survey = await _SurveyService.GetByIdAsync(id);

    if (survey is null)
      return NotFound();

    var updatedSurvey = await _SurveyService.UpdateAsync(id, model);

    if (updatedSurvey is null)
      return NotFound();

    return Ok(updatedSurvey);
  }

}