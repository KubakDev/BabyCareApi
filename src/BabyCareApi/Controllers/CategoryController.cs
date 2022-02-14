using System.ComponentModel.DataAnnotations;
using BabyCareApi.Models.Common;
using BabyCareApi.Models.Requests;
using BabyCareApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabyCareApi.Controllers;

[Authorize]
[ApiController]
[Route("v1/categories")]
public class CategoryController : ControllerBase
{

  private readonly CategoryService _CategoryService;

  public CategoryController(CategoryService categoryService)
  {
    _CategoryService = categoryService;
  }

  [HttpGet]
  public async Task<List<Category>> ListCategories([FromQuery] ListCategories model) => await _CategoryService.ListAsync(model);


  [HttpGet("{id}")]
  public async Task<ActionResult<Category>> GetById([IsBsonId] string id)
  {

    var category = await _CategoryService.GetByIdAsync(id);

    if (category is null)
      return NotFound();

    return Ok(category);

  }

  [HttpPost]
  public async Task<ActionResult<Category>> CreateCategory(CreateCategoryRequest model)
  {
    var now = DateTime.UtcNow;
    var category = model.ToCategory(now);

    await _CategoryService.CreateAsync(category);

    return CreatedAtAction(nameof(CreateCategory), category);

  }

  [HttpPut("{id}")]
  public async Task<ActionResult<Category>> UpdateCategory([IsBsonId] string id, CreateCategoryRequest model)
  {
    var category = await _CategoryService.GetByIdAsync(id);

    if (category is null)
      return NotFound();

    var updatedCategory = await _CategoryService.UpdateAsync(id, model);

    return Ok(updatedCategory);
  }

}