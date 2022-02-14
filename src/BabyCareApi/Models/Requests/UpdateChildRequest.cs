using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace BabyCareApi.Models.Requests;

public class UpdateChildRequest
{
  [Required]
  [BsonId]
  [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
  public string? ParentId { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public DateTime? Birthdate { get; set; }
  public Gender? Gender { get; set; }
  public bool? Born { get; set; }
  public IEnumerable<string>? Diseases { get; set; }

}