using System.ComponentModel.DataAnnotations;
using BabyCareApi.Models.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace BabyCareApi.Models.Requests;

public class CreateAdviceRequest
{

  [Required]
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  [Required]
  public string CategoryId { get; set; } = string.Empty;
  public IEnumerable<string> Tags { get; set; } = Array.Empty<string>();


  public Advice ToAdvice(DateTime CreationTime) => new Advice
  {
    Title = Title,
    Description = Description,
    CategoryId = CategoryId,
    Tags = Tags,
    CreatedAt = CreationTime
  };
}