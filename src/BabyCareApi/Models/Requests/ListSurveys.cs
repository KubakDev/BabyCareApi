using System.ComponentModel.DataAnnotations;

namespace BabyCareApi.Models.Requests;

public class ListSurveys
{
  [Range(0, 100)]
  public int Limit { get; set; } = 2;

  public bool? SortDescending { get; set; } = true;

  public string? SearchText { get; set; }
}