namespace BabyCareApi.Abstractions;
public interface ISortOptions
{
  /// <summary>
  /// The field to sort by.
  /// </summary>
  string? SortField { get; }

  /// <summary>
  /// Apply descending sort.
  /// </summary>
  bool? SortDescending { get; }

  string? Field => IsIdField() ? "_id" : SortField;

  bool IsEmpty() => string.IsNullOrEmpty(SortField);

  bool IsIdField()
  {
    if (string.IsNullOrEmpty(SortField))
      return false;

    return SortField == "_id";
  }

}
