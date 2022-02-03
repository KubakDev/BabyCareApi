using BabyCareApi.Abstractions;

namespace OrbitFoodApi.Models.Common;

public class SortOptions : ISortOptions
{
  public string? SortField { get; set; }

  public bool? SortDescending { get; set; }

  public SortOptions() { }

  public SortOptions(in string? sortField, in bool? sortDescending)
  {
    SortField = sortField;
    SortDescending = sortDescending;
  }
}
