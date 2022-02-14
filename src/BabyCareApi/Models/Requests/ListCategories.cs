using System.ComponentModel.DataAnnotations;

namespace BabyCareApi.Models.Requests;

public class ListCategories
{
  [Range(0, 100)]
  public int Limit { get; set; } = 2;
  public string? SearchText { get; set; }
  public bool? SortDescending { get; set; } = true;
  public string? Name { get; set; }

}