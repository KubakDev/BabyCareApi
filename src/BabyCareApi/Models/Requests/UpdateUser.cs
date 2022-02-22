using System.ComponentModel.DataAnnotations;
using BabyCareApi.Models.Common;

namespace BabyCareApi.Models.Requests;

public class UpdateUser
{
  [StringLength(64, MinimumLength = 3)]
  public string? DisplayName { get; set; }

  [StringLength(64, MinimumLength = 6)]
  public string? Password { get; set; }

  public Address? Address { get; set; }

  public UserStatus? Status { get; set; }


}

public class UpdateSelf
{
  public string? DisplayName { get; set; }
  public Address? Address { get; set; }
}