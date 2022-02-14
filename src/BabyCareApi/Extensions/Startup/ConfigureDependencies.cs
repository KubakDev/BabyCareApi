
using BabyCareApi.Services;

namespace BabyCareApi.Extensions;
public static partial class StartupConfigurations
{

  public static IServiceCollection ConfigureDependencies(this IServiceCollection services)
  {
    services
        .AddScoped<AdService>()
        .AddScoped<AdviceService>()
        .AddScoped<CategoryService>()
        .AddScoped<ChildService>()
        .AddScoped<RefreshTokenService>()
        .AddScoped<UserService>()
    ;

    services
        .AddSingleton<TokenService>()
    ;


    return services;
  }

}
