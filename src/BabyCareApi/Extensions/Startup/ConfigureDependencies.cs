
using BabyCareApi.Services;

namespace BabyCareApi.Extensions;
public static partial class StartupConfigurations
{

  public static IServiceCollection ConfigureDependencies(this IServiceCollection services)
  {
    services
        .AddScoped<UserService>()
        .AddScoped<AdService>()
        .AddScoped<RefreshTokenService>()
    ;

    services
        .AddSingleton<TokenService>()
    ;


    return services;
  }

}
