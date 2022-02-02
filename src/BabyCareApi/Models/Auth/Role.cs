namespace BabyCareApi.Models.Auth;

public enum Role
{
  Customer = 100,
  Driver = 200,
  Staff = 240,
  Admin = 900
}

public static class Roles
{
  public const string Admin = nameof(Role.Admin);
  public const string Staff = nameof(Role.Staff);
  public const string Customer = nameof(Role.Customer);

  public const string AdminOrStaff = $"{Admin},{Staff}";
  public const string AdminOrCustomer = $"{Admin},{Customer}";

}
