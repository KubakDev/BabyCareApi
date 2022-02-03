using BabyCareApi.Abstractions;
using MongoDB.Driver;

namespace BabyCareApi.Extensions;

public static class FilterExtensions
{
  public static FilterDefinition<T> FindById<T, U>(this FilterDefinitionBuilder<T> builder, U id) where T : IHasId<U> where U : notnull
      => builder.Eq(doc => doc.Id, id);

  public static FilterDefinition<T> FindById<T, U>(this FilterDefinitionBuilder<T> builder, IEnumerable<U> id) where T : IHasId<U> where U : notnull
      => builder.In(doc => doc.Id, id);
}
