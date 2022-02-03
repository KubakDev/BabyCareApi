
namespace BabyCareApi.Abstractions;
public interface IProjectionOptions
{
  bool? ExcludeFields { get; set; }

  HashSet<string>? Fields { get; set; }

  bool IsEmpty() => !(Fields?.Any() ?? false);
}
