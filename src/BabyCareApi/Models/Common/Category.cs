using MongoDB.Bson.Serialization.Attributes;

namespace BabyCareApi.Models.Common;

public class Category
{
  [BsonId]
  [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
  public string Id { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }

}