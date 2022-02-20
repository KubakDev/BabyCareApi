using System.ComponentModel.DataAnnotations;

namespace BabyCareApi.Models.Requests;

public class ListAdvices
{

  [Range(0, 100)]
  public int Limit { get; set; } = 2;
  public string? SearchText { get; set; }
  public bool? SortDescending { get; set; } = true;
  public IEnumerable<string>? Tags { get; set; }
  public string? CategoryId { get; set; }

}