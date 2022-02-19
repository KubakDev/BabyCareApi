using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace BabyCareApi.Models.Requests;

public class ListQuestions
{
  [Required]
  [BsonId]
  [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
  public string SurveyId { get; set; } = string.Empty;


  [Range(0, 100)]
  public int Limit { get; set; } = 2;

}