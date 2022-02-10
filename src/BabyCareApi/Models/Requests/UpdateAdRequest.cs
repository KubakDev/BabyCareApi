using BabyCareApi.Models.Common;

namespace BabyCareApi.Models.Requests;

public class UpdateAdRequest
{
  public string? Title { get; set; } = string.Empty;
  public IEnumerable<string>? Images { get; set; }
  public string? Description { get; set; }
  public IEnumerable<string>? Videos { get; set; }
  public AdPlacement? AdPlacement { get; set; }
  public Priority? Priority { get; set; }

}