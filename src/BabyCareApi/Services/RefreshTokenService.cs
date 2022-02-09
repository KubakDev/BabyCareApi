using BabyCareApi.Extensions;
using BabyCareApi.Models.Auth;
using MongoDB.Driver;

namespace BabyCareApi.Services;
public class RefreshTokenService
{
  public static readonly FilterDefinitionBuilder<RefreshToken> Filter = Builders<RefreshToken>.Filter;

  private readonly IMongoCollection<RefreshToken> _Collection;

  public RefreshTokenService(IMongoCollection<RefreshToken> collection)
  {
    _Collection = collection;
  }

  public async Task<RefreshToken?> GetByTokenAsync(string refreshToken)
      => await _Collection.Find(FindByToken(refreshToken)).FirstOrDefaultAsync();

  public Task CreateAsync(RefreshToken refreshToken)
      => _Collection.InsertOneAsync(refreshToken);

  public Task UpdateAsync(RefreshToken refreshToken)
      => _Collection.ReplaceOneAsync(Filter.FindById(refreshToken.Id), refreshToken);

  public Task<DeleteResult> DeleteByIdAsync(string id)
      => _Collection.DeleteOneAsync(Filter.FindById(id));

  public async Task<RefreshToken> DeleteByTokenAsync(string refreshToken, string userId)
      => await _Collection.FindOneAndDeleteAsync(FindByToken(refreshToken) & FindByUser(userId));

  private static FilterDefinition<RefreshToken> FindByToken(in string refreshToken)
      => Filter.Eq(doc => doc.Token, refreshToken);

  private static FilterDefinition<RefreshToken> FindByUser(in string userId)
      => Filter.Eq(doc => doc.UserId, userId);
}