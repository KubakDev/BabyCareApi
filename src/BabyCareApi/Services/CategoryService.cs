using BabyCareApi.Extensions;
using BabyCareApi.Models.Common;
using BabyCareApi.Models.Requests;
using MongoDB.Driver;
using OrbitFoodApi.Models.Common;

namespace BabyCareApi.Services;

public class CategoryService
{
  private static readonly FilterDefinitionBuilder<Category> Filter = Builders<Category>.Filter;
  private static readonly UpdateDefinitionBuilder<Category> Update = Builders<Category>.Update;
  private static readonly FindOneAndUpdateOptions<Category> _ReturnAfter = new()
  {
    ReturnDocument = ReturnDocument.After
  };
  public IMongoCollection<Category> Collection { get; set; }

  public CategoryService(IMongoCollection<Category> categoryCollection)
  {
    Collection = categoryCollection;

  }

  public Task CreateAsync(Category model) => Collection.InsertOneAsync(model);

  public Task<Category?> UpdateAsync(string id, CreateCategoryRequest model) => Collection.FindOneAndUpdateAsync(Filter.Eq(x => x.Id, id), Update.Set(x => x.Name, model.Name), _ReturnAfter);

  public Task<List<Category>> ListAsync(ListCategories model)
  {
    var sort = new SortOptions("Name", model.SortDescending);
    var filter = Filter.Empty;

    if (model.SearchText is not null)
      filter &= Filter.Regex(x => x.Name, $"/^{model.SearchText}");

    return Collection
    .Find(filter)
    .ApplySortOptions(sort)
    .Limit(model.Limit)
    .ToListAsync();
  }

  public async Task<Category?> GetByIdAsync(string id) => await Collection.Find(Filter.Eq(x => x.Id, id)).FirstOrDefaultAsync();

}