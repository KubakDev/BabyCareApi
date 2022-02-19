namespace BabyCareApi.Models.Auth;

public enum Role
{
  User = 100,
  AdManager = 200,
  Analyst = 240,
  Admin = 900
}

public static class Roles
{
  public const string Admin = nameof(Role.Admin);
  public const string Analyst = nameof(Role.Analyst);
  public const string User = nameof(Role.User);

  public const string AdminOrAnalyst = $"{Admin},{Analyst}";
  public const string AdminOrUser = $"{Admin},{User}";

}
