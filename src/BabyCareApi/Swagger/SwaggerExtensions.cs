using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BabyCareApi.Swagger;

public static partial class SwaggerExtensions
{
  public static IServiceCollection ConfigureSwaggerGenerator(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddSwaggerGen(c =>
    {
      c.SwaggerDoc("v1",
              new OpenApiInfo
              {
                Title = "Orbit Food Api",
                Version = $"v{Version.ApiVersion}"
              });

      if (configuration.GetValue<string?>("BasePath", null) is string serverUrl && !string.IsNullOrEmpty(serverUrl))
      {
        c.AddServer(new OpenApiServer()
        {
          Url = serverUrl
        });
      }

      //c.UseAllOfToExtendReferenceSchemas();
      c.SupportNonNullableReferenceTypes();

      MapTypes(c);
      AddSecurity(c);
      AddXmlComments(c);
      AddOperationFilters(c);

      // Use method name as operationId
      c.CustomOperationIds(apiDesc =>
      {
        return apiDesc.TryGetMethodInfo(out System.Reflection.MethodInfo methodInfo) ? methodInfo.Name : null;
      });
    });

    return services;

    static void MapTypes(SwaggerGenOptions c)
    {
      c.MapType<BsonDocument>(() => new OpenApiSchema()
      {
        Type = "object",
        AdditionalPropertiesAllowed = true,
        AdditionalProperties = new OpenApiSchema() { Type = "object" }
      });
    }

    static void AddXmlComments(SwaggerGenOptions c)
    {
      // Set the comments path for the Swagger JSON and UI.
      var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
      var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
      if (System.IO.File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
    }

    static void AddSecurity(SwaggerGenOptions c)
    {
      var securitySchema = new OpenApiSecurityScheme
      {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
          Type = ReferenceType.SecurityScheme,
          Id = "Bearer"
        }
      };

      c.AddSecurityDefinition("Bearer", securitySchema);
    }

    static void AddOperationFilters(SwaggerGenOptions c)
    {
      c.OperationFilter<SwaggerFileUploadOperationFilter>();
      c.OperationFilter<SwaggerSecurityRequirementsOperationFilter>();
    }
  }
}