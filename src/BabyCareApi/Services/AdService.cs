using BabyCareApi.Extensions;
using BabyCareApi.Models.Common;
using BabyCareApi.Models.Requests;
using MongoDB.Driver;
using OrbitFoodApi.Models.Common;

namespace BabyCareApi.Services;

public class AdService
{
  public static readonly FilterDefinitionBuilder<Ad> Filter = Builders<Ad>.Filter;
  public static readonly UpdateDefinitionBuilder<Ad> Update = Builders<Ad>.Update;
  public static readonly ProjectionDefinitionBuilder<Ad> Projection = Builders<Ad>.Projection;
  public static readonly FindOneAndUpdateOptions<Ad> _ReturnAfter = new()
  {
    ReturnDocument = ReturnDocument.After
  };

  public IMongoCollection<Ad> Collection { get; set; }

  public AdService(IMongoCollection<Ad> collection)
  {
    Collection = collection;

  }

  public Task CreateAsync(Ad model) => Collection.InsertOneAsync(model);

  public async Task<Ad?> UpdateAsync(string id, UpdateAdRequest model)
  {
    var updates = new List<UpdateDefinition<Ad>>();


    if (model.AdPlacement is not null)
      updates.Add(Update.Set(x => x.AdPlacement, model.AdPlacement));

    if (model.Priority is not null)
      updates.Add(Update.Set(x => x.AdPlacement, model.AdPlacement));

    if (model.Title is not null)
      updates.Add(Update.Set(x => x.Title, model.Title));

    if (model.Description is not null)
      updates.Add(Update.Set(x => x.Description, model.Description));

    if (updates.Any())
      return await Collection.FindOneAndUpdateAsync(Filter.Eq(x => x.Id, id), Update.Combine(updates), _ReturnAfter);

    return await GetByIdAsync(id);

  }
  public Task<List<Ad>> ListAsync(ListAds model)
  {
    var sort = new SortOptions("title", model.SortDescending);


    return Collection.Find(GetFilterDefinition(model))
                     .ApplySortOptions(sort)
                     .Limit(model.Limit)
                     .ToListAsync();
  }

  public async Task<Ad?> GetByIdAsync(string id) => await Collection.Find(Filter.Eq(x => x.Id, id)).FirstOrDefaultAsync();

  private static FilterDefinition<Ad> GetFilterDefinition(in ListAds model)
  {
    FilterDefinition<Ad> filter = Filter.Empty;

    if (model.ReachFrom is not null)
      filter &= Filter.Gt(x => x.Reach, model.ReachFrom);

    if (model.ReachTo is not null)
      filter &= Filter.Lt(x => x.Reach, model.ReachTo);

    if (!string.IsNullOrEmpty(model.SearchText))
      filter &= Filter.Regex(x => x.Title, $"/^{model.SearchText}/i");


    return filter;
  }
}

