using MongoDB.Bson.Serialization.Attributes;

namespace BabyCareApi.Models.Common;

public class Question
{
  [BsonId]
  [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
  public string Id { get; set; } = string.Empty;
  [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
  public string SurveyId { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;
  public QuestionType QuestionType { get; set; } = QuestionType.Open;
}