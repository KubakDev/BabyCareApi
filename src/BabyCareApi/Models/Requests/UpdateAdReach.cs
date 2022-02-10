using System.ComponentModel.DataAnnotations;

namespace BabyCareApi.Models.Requests;

public class UpdateAdReach
{
  [Required]
  public int Reach { get; set; }
}