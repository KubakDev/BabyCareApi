namespace BabyCareApi.Abstractions;

/// <summary>
/// Represents a resource with an identifier.
/// </summary>
public interface IHasId<T> where T : notnull
{
  /// <summary>
  /// Resource identifier.
  /// </summary>
  T Id { get; set; }
}