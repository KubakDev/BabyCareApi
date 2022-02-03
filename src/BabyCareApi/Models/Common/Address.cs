using System.ComponentModel.DataAnnotations;

namespace BabyCareApi.Models.Common;

public class Address
{
  [Required]
  public string AddressLine1 { get; set; } = String.Empty;

  public string? AddressLine2 { get; set; }
  public string PostCode { get; set; } = string.Empty;

  public override string ToString()
  {
    if (!string.IsNullOrEmpty(AddressLine2))
      return $"{AddressLine1}, {AddressLine2}, {PostCode}";

    return $"{AddressLine1}, {PostCode}";
  }

}