using MongoDB.Bson;

namespace System.ComponentModel.DataAnnotations;

public class IsBsonIdAttribute : ValidationAttribute
{
  public bool AllowEmptyString { get; set; }

  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
  {
    return value switch
    {
      string strValue => ValidateStringId(strValue),
      IEnumerable<string> arrValue => ValidateStringIds(arrValue),
      ObjectId => ValidationResult.Success,
      null => null,
      _ => new ValidationResult("Id must be of type BsonId or string.")

    };


  }
  private ValidationResult? ValidateStringId(in string? id)
  {
    if (string.IsNullOrEmpty(id))
      return AllowEmptyString
            ? ValidationResult.Success
            : new ValidationResult("An empty string cannot be used as an id string.");

    return ObjectId.TryParse(id, out _)
          ? ValidationResult.Success
          : new ValidationResult($"{id} is not a valid id string.");
  }

  private ValidationResult? ValidateStringIds(in IEnumerable<string> ids)
  {
    if (!ids.Any())
      return null;

    foreach (var id in ids)
    {
      if (ValidateStringId(id) is ValidationResult result)
        return result;
    }

    return null;
  }
}