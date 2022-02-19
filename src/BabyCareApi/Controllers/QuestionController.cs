using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using BabyCareApi.Models.Auth;
using BabyCareApi.Models.Common;
using BabyCareApi.Models.Requests;
using BabyCareApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabyCareApi.Controllers;

[ApiController]
[Authorize(Roles = Roles.Admin)]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[Route("v1/questions")]

public class QuestionController : ControllerBase
{
  private readonly QuestionService _QuestionService;

  public QuestionController(QuestionService questionService)
  {
    _QuestionService = questionService;
  }

  [HttpGet]
  public async Task<List<Question>> ListQuestions([FromQuery] ListQuestions model) => await _QuestionService.ListAsync(model);

  [HttpGet("{id}")]
  public async Task<ActionResult<Question>> GetQuestionById([IsBsonId] string id)
  {
    var question = await _QuestionService.GetByIdAsync(id);

    if (question is null)
      return NotFound();

    return
    Ok(question);
  }

  [HttpPost]
  public async Task<ActionResult<Question>> CreateQuestion(CreateQuestion model)
  {
    var question = model.ToQuestion();

    await _QuestionService.CreateAsync(question);

    return CreatedAtAction(nameof(CreateQuestion), question);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<Question>> UpdateQuestion([IsBsonId] string id, UpdateQuestionRequest model)
  {
    var question = await _QuestionService.GetByIdAsync(id);

    if (question is null)
      return NotFound();

    var updatedQuestion = await _QuestionService.UpdateAsync(id, model);

    if (updatedQuestion is null)
      return NotFound();

    return Ok(updatedQuestion);
  }
}