using BabyCareApi.Models.Common;

namespace BabyCareApi.Models.Requests;


public class CreateAdRequest
{

  public string Title { get; set; } = string.Empty;
  public IEnumerable<string>? Images { get; set; } = Array.Empty<string>();
  public string? Description { get; set; }
  public IEnumerable<string>? Videos { get; set; } = Array.Empty<string>();
  public AdPlacement AdPlacement { get; set; } = AdPlacement.Top;
  public Priority Priority { get; set; } = Priority.Medium;

  public Ad ToAd(int Reach)
         => new()
         {
           Title = Title,
           Images = Images,
           Description = Description,
           AdPlacement = AdPlacement,
           Priority = Priority,
           Reach = Reach,
           Videos = Videos
         };


}