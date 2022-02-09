namespace BabyCareApi;
/// <summary>
/// Represents the version info of the services used.
/// </summary>
public static class Version
{
  private static string _ApiVersion = string.Empty;

  /// <summary>
  /// Service API version, can only be set once.
  /// </summary>
  /// <exception cref="InvalidOperationException"/>
  public static string ApiVersion
  {
    get => _ApiVersion;
    set => _ApiVersion = string.IsNullOrEmpty(_ApiVersion)
            ? value
            : throw new InvalidOperationException("Version is already set");
  }
}