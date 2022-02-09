namespace BabyCareApi.Models.Responses;

public class AuthTokenResult
{
  public record TokenResult(string Token, DateTime ExpiresAt);

  public TokenResult Access { get; set; } = default!;

  public TokenResult? Refresh { get; set; }


}