using System.ComponentModel.DataAnnotations;
using BabyCareApi.Extensions;
using BabyCareApi.Models.Common;
using BabyCareApi.Models.Requests;
using MongoDB.Driver;
using OrbitFoodApi.Models.Common;

namespace BabyCareApi.Services;

public class ChildService
{
  private static readonly FilterDefinitionBuilder<Child> Filter = Builders<Child>.Filter;
  private static readonly UpdateDefinitionBuilder<Child> Update = Builders<Child>.Update;
  private static readonly ProjectionDefinitionBuilder<Child> Projection = Builders<Child>.Projection;
  private static readonly FindOneAndUpdateOptions<Child> _ReturnAfter = new()
  {
    ReturnDocument = ReturnDocument.After
  };

  public IMongoCollection<Child> Collection { get; set; }

  public ChildService(IMongoCollection<Child> childernCollection)
  {
    Collection = childernCollection;
  }

  public Task CreateAsync(Child child) => Collection.InsertOneAsync(child);
  public async Task<Child?> GetByIdAsync(string Id) => await Collection.Find(Filter.Eq(x => x.Id, Id)).FirstOrDefaultAsync();

  public Task<List<Child>> ListAsync(ListChildren model)
  {
    var sort = new SortOptions("displayName", model.SortDescending);

    return Collection.Find(GetFilterDefinition(model))
                     .ApplySortOptions(sort)
                     .Limit(model.Limit)
                     .ToListAsync();
  }

  public async Task<Child?> UpdateAsync(string Id, UpdateChildRequest model)
  {
    var updates = new List<UpdateDefinition<Child>>();

    if (model.Birthdate != null)
      updates.Add(Update.Set(x => x.Birthdate, model.Birthdate));

    if (model.DisplayName != null)
      updates.Add(Update.Set(x => x.DisplayName, model.DisplayName));

    if (model.Born != null)
      updates.Add(Update.Set(x => x.Born, model.Born));

    if (model.Diseases != null)
      updates.Add(Update.Set(x => x.Diseases, model.Diseases));

    if (model.Gender != null)
      updates.Add(Update.Set(x => x.Gender, model.Gender));

    if (updates.Any())
      return await Collection.FindOneAndUpdateAsync(Filter.Eq(x => x.Id, Id), Update.Combine(updates), _ReturnAfter);

    return await GetByIdAsync(Id);
  }



  private static FilterDefinition<Child> GetFilterDefinition(in ListChildren model)
  {
    var filter = Filter.Eq(x => x.ParentId, model.ParentId);
    // var textSearch = Filter.Text(model.SearchText);
    if (!string.IsNullOrEmpty(model.SearchText))
      filter &= Filter.Regex(x => x.DisplayName, $"/^{model.SearchText}/i");

    if (string.IsNullOrEmpty(model.LastSeenDisplayName))
      return filter;

    if (model.BornFrom is not null)
      return filter &= Filter.Gt(x => x.Birthdate, model.BornFrom);

    if (model.BornTo is not null)
      return filter &= Filter.Lt(x => x.Birthdate, model.BornTo);

    if (model.Diseases is not null)
    {
      for (int i = 0; i < model.Diseases.Count; i++)
      {
        string Disease = model.Diseases[i];
        filter &= Filter.ElemMatch(x => x.Diseases, disease => disease.StartsWith(Disease));
      }

    }

    filter &= model.SortDescending is true
       ? Filter.Lt(x => x.DisplayName, model.LastSeenDisplayName)
       : Filter.Gt(x => x.DisplayName, model.LastSeenDisplayName);

    return filter;
  }

}