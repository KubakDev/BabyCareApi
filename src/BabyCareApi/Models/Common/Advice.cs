using MongoDB.Bson.Serialization.Attributes;

namespace BabyCareApi.Models.Common;

public class Advice
{

  [BsonId]
  [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
  public string Id { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public IEnumerable<string> Tags { get; set; } = Array.Empty<string>();
  public string CategoryId { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }

}