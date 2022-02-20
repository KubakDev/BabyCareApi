using BabyCareApi.Extensions;
using BabyCareApi.Models.Common;
using BabyCareApi.Models.Requests;
using MongoDB.Bson;
using MongoDB.Driver;
using OrbitFoodApi.Models.Common;

namespace BabyCareApi.Service;


public class SurveyService
{
  private static readonly FilterDefinitionBuilder<Survey> Filter = Builders<Survey>.Filter;
  private static readonly UpdateDefinitionBuilder<Survey> Update = Builders<Survey>.Update;
  private static readonly ProjectionDefinitionBuilder<Survey> Projection = Builders<Survey>.Projection;
  private static readonly FindOneAndUpdateOptions<Survey> _ReturnAfter = new()
  {
    ReturnDocument = ReturnDocument.After
  };
  public IMongoCollection<Survey> Collection { get; set; }

  public SurveyService(IMongoCollection<Survey> surveyCollection)
  {
    Collection = surveyCollection;

  }

  public Task CreateAsync(Survey model) => Collection.InsertOneAsync(model);

  public async Task<Survey?> GetByIdAsync(string id) => await Collection.Find(Filter.Eq(x => x.Id, id)).FirstOrDefaultAsync();

  public Task<List<Survey>> ListAsync(ListSurveys model)
  {

    var sort = new SortOptions("title", model.SortDescending);


    return Collection
    .Find(GetFilterDefinition(model))
    .ApplySortOptions(sort)
    .Project<Survey>(Projection.Exclude("questions"))
    .Limit(model.Limit)
    .ToListAsync();
  }

  public async Task<Survey?> UpdateAsync(string id, UpdateSurveyRequest model)
  {
    var updates = new List<UpdateDefinition<Survey>>();

    if (model.Questions is not null)
      updates.Add(Update.Set(x => x.Questions, model.Questions));

    if (model.Tags is not null)
      updates.Add(Update.Set(x => x.Tags, model.Tags));

    if (updates.Any())
      return await Collection.FindOneAndUpdateAsync(Filter.Eq(x => x.Id, id), Update.Combine(updates), _ReturnAfter);

    return await GetByIdAsync(id);

  }

  private static FilterDefinition<Survey> GetFilterDefinition(ListSurveys model)
  {
    FilterDefinition<Survey> filter = Filter.Empty;

    if (model.SearchText is not null)
      filter &= Filter.Regex(x => x.Title, $"/^{model.SearchText}/i");

    return filter;


  }



}