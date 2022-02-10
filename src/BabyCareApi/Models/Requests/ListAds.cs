using System.ComponentModel.DataAnnotations;
using BabyCareApi.Models.Common;

namespace BabyCareApi.Models.Requests;


// <summary>
// Represents a request to list ads
// </summary>
public class ListAds
{


  [Range(0, 100)]
  public int Limit { get; set; } = 2;

  public string? SearchText { get; set; }
  public int? ReachFrom { get; set; }
  public int? ReachTo { get; set; }

  public bool? SortDescending { get; set; } = true;

}