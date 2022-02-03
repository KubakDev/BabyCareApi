using System.ComponentModel.DataAnnotations;
using BabyCareApi.Models.Auth;

namespace BabyCareApi.Models.Requests;

/// <summary>
/// Represents a request to list users.
/// </summary>

public class ListUsers
{
  [Range(0, 100)]
  public int Limit { get; set; } = 2;

  [Required]
  public Role Role { get; set; }

  public string? SearchText { get; set; }

  // <summary>
  // Username of the last seen user. (Used for infinite scorll)
  // </summary>
  public string? LastSeenUsername { get; set; }
  public bool? SortDescending { get; set; } = true;


}