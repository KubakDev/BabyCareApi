using System.ComponentModel.DataAnnotations;

namespace BabyCareApi.Settings;

public class MongoDbOptions
{
  public const string Config = "MongoDB";
  [Required]
  public string DatabaseName { get; set; } = "test";
  [Required]
  public string ConnectionString { get; set; } = string.Empty;
}