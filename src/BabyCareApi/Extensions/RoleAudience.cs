using BabyCareApi.Models.Auth;

namespace BabyCareApi.Extensions;

public static class RoleAudience
{
  public static Role[] GetRoles(this Audience audience) =>
  audience switch
  {
    Audience.CustomerApp => new[] { Role.Customer },
    Audience.AdminWebPanel => new[] { Role.Admin },
    _ => new[] { Role.Driver, Role.Staff }
  };
  public static Audience GetAudience(this Role role) =>
  role switch
  {
    Role.Admin => Audience.AdminWebPanel,
    Role.Customer => Audience.CustomerApp,
    _ => Audience.StaffApp,
  };
}