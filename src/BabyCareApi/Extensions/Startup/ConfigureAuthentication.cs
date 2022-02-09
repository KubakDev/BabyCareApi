using BabyCareApi.Models.Auth;
using BabyCareApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BabyCareApi.Extensions;


public static partial class StartupConfigurations
{
  public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration config)
  {
    config = config.GetSection(AuthOptions.Config);

    var auth = config.Get<AuthOptions>() ?? new AuthOptions();

    services
      .AddOptions<AuthOptions>()
      .Bind(config)
      .ValidateDataAnnotations();

    services.AddAuthentication(ConfigureAuthentication)
      .AddJwtBearer(ConfigureJwtBearer);


    return services;

    void ConfigureJwtBearer(JwtBearerOptions options)
    {
      options.SaveToken = true;
      options.RequireHttpsMetadata = true;
      options.TokenValidationParameters = new()
      {
        ClockSkew = TimeSpan.Zero,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = auth.Issuer,
        ValidAudiences = new[]
        {
          Audience.UserApp.ToString(),
          Audience.AdminWebPanel.ToString()
        },
        IssuerSigningKey = new SymmetricSecurityKey(auth.SecretBytes)
      };

      options.Events = new JwtBearerEvents
      {
        OnAuthenticationFailed = context =>
        {
          if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            context.Response.Headers.Add("Token-Expired", "true");

          return Task.CompletedTask;
        }
      };
    }

    static void ConfigureAuthentication(AuthenticationOptions options)
    {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }

  }
}