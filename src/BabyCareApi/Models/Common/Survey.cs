using MongoDB.Bson.Serialization.Attributes;

namespace BabyCareApi.Models.Common;

public class Survey
{
  [BsonId]
  [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
  public string Id { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;
  public IEnumerable<Question>? Questions { get; set; }
  public IEnumerable<string>? Tags { get; set; }
  public DateTime CreatedAt { get; set; }
  public int ResponseNumber { get; set; } = 0;
}