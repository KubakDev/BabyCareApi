using System.ComponentModel.DataAnnotations;
using BabyCareApi.Models.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace BabyCareApi.Models.Requests;

public class CreateChildRequest
{
  [Required]
  [BsonId]
  [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
  public string ParentId { get; set; } = string.Empty;
  [Required]
  public string DisplayName { get; set; }
  public DateTime? Birthdate { get; set; }
  [Required]
  public bool Born { get; set; }
  public Gender? Gender { get; set; }
  public IEnumerable<string> Diseases { get; set; } = Array.Empty<string>();

  public Child ToChild(DateTime creationTime)
      => new()
      {
        DisplayName = DisplayName,
        Birthdate = Birthdate,
        Born = Born,
        Gender = Gender,
        CreatedAt = creationTime,
        UpdatedAt = creationTime,
        Diseases = Diseases,
        ParentId = ParentId
      };

}