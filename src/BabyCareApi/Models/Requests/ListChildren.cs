using System.ComponentModel.DataAnnotations;
using BabyCareApi.Models.Common;

namespace BabyCareApi.Models.Requests;


// <summary>
// Represents a request to list ads
// </summary>
public class ListChildren
{


  [Range(0, 100)]
  public int Limit { get; set; } = 2;
  public string? SearchText { get; set; }
  public string? LastSeenDisplayName { get; set; }
  public bool? SortDescending { get; set; } = true;
  public bool? Born { get; set; }
  public DateTime? BornFrom { get; set; }
  public DateTime? BornTo { get; set; }
  public Gender? Gender { get; set; }
  public List<string>? Diseases { get; set; }


}