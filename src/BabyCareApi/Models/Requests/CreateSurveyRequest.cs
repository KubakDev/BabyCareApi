using System.ComponentModel.DataAnnotations;

namespace BabyCareApi.Models.Common;

public class CreateSurveyRequest
{
  [Required]
  public string Title { get; set; } = string.Empty;
  public HashSet<string>? Tags { get; set; }

  public Survey ToSurvey(DateTime CreationDate) => new()
  {
    CreatedAt = CreationDate,
    ResponseNumber = 0,
    Tags = Tags,
    Title = Title
  };
}