using System.ComponentModel.DataAnnotations;
using BabyCareApi.Models.Common;

namespace BabyCareApi.Models.Requests;


public class CreateCategoryRequest
{

  [Required]
  public string Name { get; set; } = string.Empty;

  public Category ToCategory(DateTime CreationTime) => new()
  {
    Name = Name,
    CreatedAt = CreationTime
  };
}