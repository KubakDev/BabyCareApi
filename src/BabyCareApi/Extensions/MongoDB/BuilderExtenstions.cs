using MongoDB.Driver;
using BabyCareApi.Abstractions;

namespace BabyCareApi.Extensions
{
  public static class BuilderExtensions
  {
    public static SortDefinition<TDoc> BuildSortDefinition<TDoc>(this ISortOptions sortOptions)
    {
      if (sortOptions is null || sortOptions.IsEmpty())
        throw new ArgumentNullException(paramName: nameof(sortOptions));

      return (sortOptions.SortDescending ?? false)
       ? Builders<TDoc>.Sort.Descending(sortOptions.Field)
       : Builders<TDoc>.Sort.Ascending(sortOptions.Field);
    }

    public static ProjectionDefinition<TDoc> BuildProjectionDefinition<TDoc>(this IProjectionOptions projectionOptions)
        => BuildProjectionWithoutTextScoreOptions<TDoc>(projectionOptions);

    private static ProjectionDefinition<TDoc> BuildProjectionWithoutTextScoreOptions<TDoc>(IProjectionOptions projectionOptions)
    {
      if (projectionOptions is null || projectionOptions.IsEmpty())
        throw new ArgumentNullException(paramName: nameof(projectionOptions));

      var fields = projectionOptions.Fields!;

      var projection = projectionOptions.ExcludeFields ?? false
          ? Builders<TDoc>.Projection.Exclude(fields.First())
          : Builders<TDoc>.Projection.Include(fields.First());

      foreach (var field in fields.Skip(1))
      {
        projection = projectionOptions.ExcludeFields ?? false
            ? projection.Exclude(field)
            : projection.Include(field);
      }

      return projection;
    }

  }
}
