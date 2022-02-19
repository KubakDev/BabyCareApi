using MongoDB.Bson.Serialization.Attributes;

namespace BabyCareApi.Models.Common;

public class Child
{
  [BsonId]
  [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
  public string Id { get; set; } = string.Empty;
  [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
  public string ParentId { get; set; } = string.Empty;
  public string DisplayName { get; set; } = string.Empty;
  public DateTime? Birthdate { get; set; }
  public Gender? Gender { get; set; }
  public bool Born { get; set; } = false;
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public IEnumerable<string> Diseases { get; set; } = Array.Empty<string>();


}