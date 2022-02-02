using System.Text.Json.Serialization;
using BabyCareApi.Models.Auth;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BabyCareApi.Models;

public class User
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string Id { get; set; } = string.Empty;
  public string Username { get; set; } = string.Empty;
  public string DisplayName { get; set; } = string.Empty;
  public string? PhoneNumber { get; set; }

  [JsonIgnore]
  public string? Password { get; set; }
  public DateTime? CreatedAt { get; set; }
  public bool IsVerified { get; set; }
  public Role Role { get; set; }
}