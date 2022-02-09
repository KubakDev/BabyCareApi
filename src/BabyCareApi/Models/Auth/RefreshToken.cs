using System.Text.Json.Serialization;
using BabyCareApi.Abstractions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BabyCareApi.Models.Auth;


public class RefreshToken : IHasId<string>
{

  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string Id { get; set; } = string.Empty;

  [JsonIgnore]
  [BsonRepresentation(BsonType.ObjectId)]
  public string UserId { get; set; } = string.Empty;

  [JsonIgnore]
  public string JwtId { get; set; } = string.Empty;

  public string Token { get; set; } = string.Empty;

  public Audience Audience { get; set; }
  public DateTime ExpiresAt { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime LastRefreshedAt { get; set; }

}