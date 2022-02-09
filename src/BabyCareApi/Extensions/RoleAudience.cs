using BabyCareApi.Models.Auth;

namespace BabyCareApi.Extensions;

public static class RoleAudience
{
  public static Role[] GetRoles(this Audience audience) =>
  audience switch
  {
    Audience.UserApp => new[] { Role.User },
    Audience.AdminWebPanel => new[] { Role.Admin },
    _ => new[] { Role.AdManager, Role.Admin, Role.Analyst }
  };
  public static Audience GetAudience(this Role role) =>
  role switch
  {
    Role.Admin => Audience.AdminWebPanel,
    Role.User => Audience.UserApp,
  };
}