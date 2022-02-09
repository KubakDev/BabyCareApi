using System.Security.Claims;
using BabyCareApi.Models.Auth;

namespace BabyCareApi.Extensions;

public static class ClaimsPrincipalExtensions
{
  public static Role GetRole(this ClaimsPrincipal user)
  {
    var roleStr = user.FindFirstValue(ClaimTypes.Role);

    if (string.IsNullOrEmpty(roleStr))
      return Role.User;

    return Enum.Parse<Role>(roleStr);


  }

  public static string GetJti(this ClaimsPrincipal user)
          => user.FindFirstValue("jti");

  public static string GetId(this ClaimsPrincipal user)
      => user.FindFirstValue(ClaimTypes.NameIdentifier);

  public static bool HasId(this ClaimsPrincipal user, in string id)
      => user.FindFirstValue("sub") == id;
}