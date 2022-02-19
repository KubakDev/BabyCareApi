namespace BabyCareApi.Models.Requests;

public class UpdateQuestionRequest
{
  public string? Title { get; set; } = string.Empty;
  public QuestionType? QuestionType { get; set; }
}