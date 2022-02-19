using BabyCareApi.Models.Common;

namespace BabyCareApi.Models.Requests;

public class UpdateSurveyRequest
{
  public string? Title { get; set; } = string.Empty;
  public IEnumerable<Question>? Questions { get; set; }
  public IEnumerable<string>? Tags { get; set; }
}