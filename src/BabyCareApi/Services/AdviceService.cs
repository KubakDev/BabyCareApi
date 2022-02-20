using BabyCareApi.Extensions;
using BabyCareApi.Models.Common;
using BabyCareApi.Models.Requests;
using MongoDB.Driver;
using OrbitFoodApi.Models.Common;

namespace BabyCareApi.Services;

public class AdviceService
{
  private static readonly FilterDefinitionBuilder<Advice> Filter = Builders<Advice>.Filter;
  private static readonly UpdateDefinitionBuilder<Advice> Update = Builders<Advice>.Update;
  private static readonly FindOneAndUpdateOptions<Advice> _ReturnAfter = new()
  {
    ReturnDocument = ReturnDocument.After
  };

  public IMongoCollection<Advice> Collection { get; set; }
  public AdviceService(IMongoCollection<Advice> adviceCollection)
  {
    Collection = adviceCollection;
  }

  public Task CreateAsync(Advice model) => Collection.InsertOneAsync(model);


  public async Task<Advice?> UpdateAsync(string id, CreateAdviceRequest model)
  {
    var updates = new List<UpdateDefinition<Advice>>();

    if (model.Title is not null)
      updates.Add(Update.Set(x => x.Title, model.Title));

    if (model.Description is not null)
      updates.Add(Update.Set(x => x.Description, model.Description));

    if (model.Tags is not null)
      updates.Add(Update.Set(x => x.Tags, model.Tags));

    if (model.CategoryId is not null)
      updates.Add(Update.Set(x => x.CategoryId, model.CategoryId));

    if (updates.Any())
      return await Collection.FindOneAndUpdateAsync(Filter.Eq(x => x.Id, id), Update.Combine(updates), _ReturnAfter);

    return await GetByIdAsync(id);

  }

  public Task<List<Advice>> ListAsync(ListAdvices model)
  {
    var sort = new SortOptions("displayName", model.SortDescending);
    return Collection.Find(GetFilterDefinition(model))
                     .ApplySortOptions(sort)
                     .Limit(model.Limit)
                     .ToListAsync();
  }

  public async Task<Advice?> GetByIdAsync(string id) => await Collection.Find(Filter.Eq(x => x.Id, id)).FirstOrDefaultAsync();

  private static FilterDefinition<Advice> GetFilterDefinition(in ListAdvices model)
  {
    FilterDefinition<Advice> filter = Filter.Empty;

    if (!string.IsNullOrEmpty(model.SearchText))
      filter &= Filter.Regex(x => x.Title, $"/^{model.SearchText}");

    if (model.Tags is not null)
      filter &= Filter.All(x => x.Tags, model.Tags);

    if (model.CategoryId is not null)
      filter &= Filter.Eq(x => x.CategoryId, model.CategoryId);

    return filter;
  }
}