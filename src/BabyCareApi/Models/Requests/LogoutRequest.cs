using System.ComponentModel.DataAnnotations;

namespace BabyCareApi.Models.Requests;

public class LogoutRequest
{
  [Required]
  public string RefreshToken { get; set; } = string.Empty;


}