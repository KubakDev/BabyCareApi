namespace BabyCareApi.Models.Responses;

public class LoginResult
{

  public User User { get; set; } = default!;

  public AuthTokenResult Auth { get; set; } = default!;



}