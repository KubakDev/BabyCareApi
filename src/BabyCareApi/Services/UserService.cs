using BabyCareApi.Extensions;
using BabyCareApi.Models;
using BabyCareApi.Models.Auth;
using BabyCareApi.Models.Requests;
using MongoDB.Driver;
using OrbitFoodApi.Models.Common;

namespace BabyCareApi.Services;

public class UserService
{
  private static FilterDefinitionBuilder<User> Filter => Builders<User>.Filter;
  private static UpdateDefinitionBuilder<User> Update => Builders<User>.Update;
  private static FindOneAndReplaceOptions<User, User> _ReturnAfter => new() { ReturnDocument = ReturnDocument.After, Projection = _WithoutPassword, };

  public static readonly ProjectionDefinitionBuilder<User> Projection = Builders<User>.Projection;
  private static readonly ProjectionDefinition<User> _WithoutPassword = Projection.Exclude(doc => doc.Password);

  public IMongoCollection<User> Collection { get; init; }

  public UserService(IMongoCollection<User> usersCollection)
  {
    Collection = usersCollection;

  }

  public Task<bool> UsernameExistsAsync(string username)
  {
    var filter = Filter.Eq(doc => doc.Username, username);
    return Collection.Find(filter).AnyAsync();
  }


  public async Task<User?> LoginAsync(string username, string password, Audience audience)
  {
    var filter = Filter.Eq(doc => doc.Username, username) & Filter.Eq(doc => doc.Password, password) & Filter.In(doc => doc.Role, audience.GetRoles());
    var user = await Collection.Find(filter).FirstOrDefaultAsync();

    if (user is not null) return user;
    return null;

  }

  public Task<UpdateResult> SetVerifiedAsync(string id)
  {
    var filter = Filter.Eq(doc => doc.Id, id);
    var update = Update.Set(doc => doc.IsVerified, true);

    return Collection.UpdateOneAsync(filter, update);

  }

  public Task<UpdateResult> SetPasswordAsync(string id, string password)
  {
    var filter = Filter.Eq(doc => doc.Id, id);
    var update = Update.Set(doc => doc.Password, password);

    return Collection.UpdateOneAsync(filter, update);

  }

  public Task CreateAsync(User user) => Collection.InsertOneAsync(user);

  public async Task<User?> UpdateAsync(string id, UpdateUser model)
  {
    var updates = new List<UpdateDefinition<User>>();

    if (model.Username != null)
      updates.Add(Update.Set(x => x.Username, model.Username));

    if (model.DisplayName != null)
      updates.Add(Update.Set(x => x.Username, model.Username));

    if (model.Password != null)
      updates.Add(Update.Set(x => x.Password, model.Password));

    if (model.Status != null)
      updates.Add(Update.Set(x => x.Status, model.Status));

    if (model.Address != null)
      updates.Add(Update.Set(x => x.Address, model.Address));

    if (updates.Any())
      return await Collection.FindOneAndUpdateAsync<User>(Filter.Eq(x => x.Id, id), Update.Combine(updates), new FindOneAndUpdateOptions<User, User>() { ReturnDocument = ReturnDocument.After });

    return await GetByIdAsync(id);

  }



  public Task<List<User>> ListAsync(ListUsers model)
  {
    var sort = new SortOptions("username", model.SortDescending);

    return Collection.Find(GetFilterDefinition(model))
                          .Project<User>(_WithoutPassword)
                          .ApplySortOptions(sort)
                          .Limit(model.Limit)
                          .ToListAsync();
  }

  private static FilterDefinition<User> GetFilterDefinition(in ListUsers model)
  {
    var filter = FilterByRole(model.Role);

    if (!string.IsNullOrEmpty(model.SearchText))
      filter &= Filter.Regex(doc => doc.Username, $"/^{model.SearchText}/i");

    if (string.IsNullOrEmpty(model.LastSeenUsername))
      return filter;

    filter &= model.SortDescending is true
    ? Filter.Lt(doc => doc.Username, model.LastSeenUsername)
    : Filter.Gt(doc => doc.Username, model.LastSeenUsername);


    return filter;

  }

  public async Task<User?> GetByIdAsync(string id)
          => await Collection.Find(Filter.Eq(doc => doc.Id, id)).FirstOrDefaultAsync();


  private static FilterDefinition<User> FilterByRole(Role role) =>
     Filter.Eq(doc => doc.Role, role);


}