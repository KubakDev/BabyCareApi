using BabyCareApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;


services.AddControllers();

services
.ConfigureMongoDB(config)
.AddEndpointsApiExplorer()
.AddSwaggerGen()
;

var app = builder.Build();
app
.UseSwagger()
.UseSwaggerUI()
.UseHttpsRedirection()
.UseAuthorization();

app.MapControllers();

app.Run();
