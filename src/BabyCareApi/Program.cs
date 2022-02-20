using System.Text.Json;
using System.Text.Json.Serialization;
using BabyCareApi.Common;
using BabyCareApi.Extensions;
using BabyCareApi.Swagger;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;


services
.ConfigureMongoDB(config)
.AddEndpointsApiExplorer()
.AddSwaggerGen()
.ConfigureAuthentication(config);

services
            .AddHttpClient()
            .ConfigureSwaggerGenerator(config)
            .AddControllers()
            .AddJsonOptions(x =>
            {
              x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
              x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

              x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

services.ConfigureDependencies();

var app = builder.Build();
app
.UseSwagger()
.UseSwaggerUI()
.UseHttpsRedirection()
.UseAuthentication()
.UseAuthorization();

app.MapControllers();

app.Run();
