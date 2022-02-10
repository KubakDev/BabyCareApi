using BabyCareApi.Models;
using BabyCareApi.Models.Auth;
using BabyCareApi.Models.Common;
using BabyCareApi.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BabyCareApi.Extensions;

public static partial class StartupConfigurations
{
  public static IServiceCollection ConfigureMongoDB(this IServiceCollection services, IConfiguration config)
  {
    config = config.GetSection(MongoDbOptions.Config);

    services.AddOptions<MongoDbOptions>()
    .Bind(config)
    .ValidateDataAnnotations();

    return services
    .AddSingleton(ClientImplementationFactory)
    .AddSingleton(DatabaseImplementationFactory)
    .AddSingleton(sp => CollectionImplementationFactory<User>(sp, "users"))
    .AddSingleton(sp => CollectionImplementationFactory<RefreshToken>(sp, "refresh_tokens"))
    .AddSingleton(sp => CollectionImplementationFactory<Ad>(sp, "ads"))

    ;
  }

  public static IMongoClient ClientImplementationFactory(IServiceProvider serviceProvider)
  {
    var config = serviceProvider.GetRequiredService<IOptions<MongoDbOptions>>();

    var clientSettings = MongoClientSettings.FromConnectionString(config.Value.ConnectionString);

    return new MongoClient(clientSettings);
  }

  public static IMongoDatabase DatabaseImplementationFactory(IServiceProvider serviceProvider)
  {
    var config = serviceProvider.GetRequiredService<IOptions<MongoDbOptions>>();
    var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();

    return mongoClient.GetDatabase(config.Value.DatabaseName);
  }

  public static IMongoCollection<T> CollectionImplementationFactory<T>(IServiceProvider serviceProvider, in string collectionName)
  {
    var mongoDatabase = serviceProvider.GetRequiredService<IMongoDatabase>();

    return mongoDatabase.GetCollection<T>(collectionName);
  }
}