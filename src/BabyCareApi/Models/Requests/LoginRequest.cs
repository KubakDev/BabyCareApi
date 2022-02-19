using System.ComponentModel.DataAnnotations;
using BabyCareApi.Models.Auth;

namespace BabyCareApi.Models.Requests;

public class LoginRequest
{

  [Required]
  public string Username { get; set; } = string.Empty;

  [Required]
  public string Password { get; set; } = string.Empty;

  [Required]
  public Audience Audience { get; set; }

}