using System.ComponentModel.DataAnnotations;
using BabyCareApi.Models;
using BabyCareApi.Models.Auth;
using BabyCareApi.Models.Common;

namespace BabyCareteApi.Models.Requests;

public class CreateUser : IValidatableObject
{
  [Required]
  [StringLength(20, MinimumLength = 3)]
  public string Username { get; set; } = string.Empty;

  [Required]
  [StringLength(64, MinimumLength = 3)]
  public string DisplayName { get; set; } = string.Empty;

  [Required]
  [StringLength(64, MinimumLength = 6)]
  public string Password { get; set; } = string.Empty;

  [Required]
  public Role Role { get; set; }

  public string Gender { get; set; } = string.Empty;

  public PregnancyState PregnancyState { get; set; }
  public User ToUser(DateTime creationTime)
        => new()
        {
          Role = Role,
          Username = Username,
          DisplayName = DisplayName,
          Password = Password,
          CreatedAt = creationTime,
          IsVerified = true,
        };

  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
    // yield is used for example if we had more validations we could return them with yield, but without it the return statement will not return the other validations below the returned(first) one
    if (Role is Role.User)
      yield return new ValidationResult("User with role 'User' must be registered.", new[] { nameof(Role) });

  }

}