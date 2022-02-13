using BabyCareApi.Extensions;
using BabyCareApi.Models.Auth;
using BabyCareApi.Models.Common;
using BabyCareApi.Models.Requests;
using MongoDB.Driver;
using OrbitFoodApi.Models.Common;

namespace BabyCareApi.Services;
public class UserService
{
  public static readonly FilterDefinitionBuilder<User> Filter = Builders<User>.Filter;
  public static readonly UpdateDefinitionBuilder<User> Update = Builders<User>.Update;
  public static readonly ProjectionDefinitionBuilder<User> Projection = Builders<User>.Projection;
  private static readonly ProjectionDefinition<User> _WithoutPassword = Projection.Exclude(doc => doc.Password);

  private static readonly FindOneAndUpdateOptions<User> _ReturnAfter = new()
  {
    ReturnDocument = ReturnDocument.After,
    Projection = _WithoutPassword,
  };

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

  public Task<bool> PhoneNumberExistsAsync(string phoneNumber)
  {
    var filter = Filter.Eq(doc => doc.PhoneNumber, phoneNumber);
    return Collection.Find(filter).AnyAsync();
  }

  public async Task<User?> LoginAsync(string username, string password, Audience audience)
  {
    var filter = Filter.Eq(doc => doc.Username, username) & Filter.In(doc => doc.Role, audience.GetRoles());
    var user = await Collection.Find(filter).FirstOrDefaultAsync();

    if (user != null && user.Password == password)
      return user;

    return null;
  }


  public Task<UpdateResult> SetVerifiedAsync(string id)
  {
    var filter = Filter.Eq(x => x.Id, id);
    var update = Update.Set(doc => doc.IsVerified, true);

    return Collection.UpdateOneAsync(filter, update);
  }

  public Task<UpdateResult> SetPasswordAsync(string id, string hashedPassword)
  {
    var filter = Filter.Eq(x => x.Id, id);
    var update = Update.Set(doc => doc.Password, hashedPassword);

    return Collection.UpdateOneAsync(filter, update);
  }

  public Task<UpdateResult> ChangePhoneNumberAsync(string id, string phoneNumber)
  {
    var filter = Filter.Eq(x => x.Id, id);
    var update = Update.Set(x => x.PhoneNumber, phoneNumber)
                       .Unset(x => x.IsVerified);

    return Collection.UpdateOneAsync(filter, update);
  }

  public Task CreateAsync(User user)
      => Collection.InsertOneAsync(user);

  public async Task<User?> UpdateAsync(string id, UpdateUser model)
  {
    var updates = new List<UpdateDefinition<User>>();

    if (model.Username != null)
      updates.Add(Update.Set(x => x.Username, model.Username));

    if (model.DisplayName != null)
      updates.Add(Update.Set(x => x.DisplayName, model.DisplayName));

    if (model.Address != null)
      updates.Add(Update.Set(x => x.Address, model.Address));

    if (model.Status != null)
      updates.Add(Update.Set(x => x.Status, model.Status));

    if (model.Password != null)
      updates.Add(Update.Set(x => x.Password, model.Password));

    if (updates.Any())
      return await Collection.FindOneAndUpdateAsync(Filter.Eq(x => x.Id, id), Update.Combine(updates), _ReturnAfter);

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

  public Task<List<User>> ListByIdAsync(IEnumerable<string> ids)
      => Collection.Find(Filter.In(x => x.Id, ids))
                   .Project<User>(_WithoutPassword)
                   .ToListAsync();

  public async Task<User?> GetByIdAsync(string id)
      => await Collection.Find(Filter.Eq(x => x.Id, id)).FirstOrDefaultAsync();

  public async Task<User?> GetByPhoneNumberAsync(string number)
      => await Collection.Find(Filter.Eq(x => x.PhoneNumber, number)).FirstOrDefaultAsync();

  private static FilterDefinition<User> GetFilterDefinition(in ListUsers model)
  {
    var filter = FilterByRole(model.Role);

    if (!string.IsNullOrEmpty(model.SearchText))
      filter &= Filter.Regex(x => x.Username, $"/^{model.SearchText}/i");

    if (model.IsVerified is not null)
      filter &= Filter.Eq(x => x.IsVerified, model.IsVerified);

    if (model.Status is not null)
      filter &= Filter.Eq(x => x.Status, model.Status);

    if (string.IsNullOrEmpty(model.LastSeenUsername))
      return filter;

    filter &= model.SortDescending is true
       ? Filter.Lt(x => x.Username, model.LastSeenUsername)
       : Filter.Gt(x => x.Username, model.LastSeenUsername);

    return filter;
  }

  private static FilterDefinition<User> FilterByRole(in Role role)
      => Filter.Eq(doc => doc.Role, role);

}