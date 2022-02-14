using System.Text.Json.Serialization;
using BabyCareApi.Models.Auth;
using BabyCareApi.Models.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BabyCareApi.Models.Common;

public class User
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string Id { get; set; } = string.Empty;

  public string Username { get; set; } = string.Empty;

  public string DisplayName { get; set; } = string.Empty;

  public Address? Address { get; set; }

  public string? PhoneNumber { get; set; }

  [JsonIgnore]
  public string? Password { get; set; }

  public Role Role { get; set; }

  public bool IsVerified { get; set; }

  public UserStatus Status { get; set; }

  public DateTime? CreatedAt { get; set; }
  public IEnumerable<string> Diseases { get; set; } = Array.Empty<string>();

}