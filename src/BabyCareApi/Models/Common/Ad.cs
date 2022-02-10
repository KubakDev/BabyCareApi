using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BabyCareApi.Models.Common;

public class Ad
{
  [BsonRepresentation(BsonType.ObjectId)]
  [BsonId]
  public string Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public IEnumerable<string>? Images { get; set; } = Array.Empty<string>();
  public string? Description { get; set; }
  public IEnumerable<string>? Videos { get; set; } = Array.Empty<string>();
  public AdPlacement AdPlacement { get; set; } = AdPlacement.Top;
  public Priority Priority { get; set; } = Priority.Medium;
  public int Reach { get; set; } = 0;


}