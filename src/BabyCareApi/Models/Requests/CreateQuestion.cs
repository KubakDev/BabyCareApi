using System.ComponentModel.DataAnnotations;
using BabyCareApi.Models.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace BabyCareApi.Models.Requests;

public class CreateQuestion
{
  [Required]
  public string Title { get; set; } = string.Empty;
  [Required]
  [BsonId]
  [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
  public string SurveyId { get; set; } = string.Empty;
  public QuestionType QuestionType { get; set; }
  public Question ToQuestion()
      => new()
      {
        Title = Title,
        SurveyId = SurveyId,
        QuestionType = QuestionType
      };

}