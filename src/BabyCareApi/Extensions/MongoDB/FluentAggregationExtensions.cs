using MongoDB.Bson;
using MongoDB.Driver;
using BabyCareApi.Abstractions;

namespace BabyCareApi.Extensions;
public static class FluentAggregateExtensions
{
  public static IAggregateFluent<TDoc> ApplySortOptions<TDoc>(this IAggregateFluent<TDoc> aggregate, ISortOptions? sortOptions, string? thenByAsc = null)
  {
    if (sortOptions is null || sortOptions.IsEmpty())
      return aggregate;

    var sortDef = sortOptions.BuildSortDefinition<TDoc>();

    if (!string.IsNullOrEmpty(thenByAsc) && sortOptions.Field != thenByAsc)
      sortDef = sortDef.Ascending(thenByAsc);

    return aggregate.Sort(sortDef);
  }

  public static IAggregateFluent<TProj> ApplyProjectionOptions<TDoc, TProj>(this IAggregateFluent<TDoc> aggregate, IProjectionOptions? projectionOptions)
  {
    if (projectionOptions is null || projectionOptions.IsEmpty())
      return aggregate.As<TProj>();

    return aggregate.Project<TProj>(projectionOptions.BuildProjectionDefinition<TDoc>());
  }

  public static IAggregateFluent<TDoc> Limit<TDoc>(this IAggregateFluent<TDoc> aggregate, int? limit)
  {
    if (limit is int actualLimit)
      return aggregate.Limit(actualLimit);

    return aggregate;
  }

  public static IAggregateFluent<TProj> As<TDoc, TProj>(this IAggregateFluent<TDoc> aggregate)
      => aggregate.AppendStage<TProj>(new BsonDocument());

  #region Aggregate: $unset 

  public static IAggregateFluent<TDoc> Unset<TDoc>(this IAggregateFluent<TDoc> aggregate, params string[] fields)
      => Unset<TDoc, TDoc>(aggregate, fields);

  public static IAggregateFluent<TProj> Unset<TDoc, TProj>(this IAggregateFluent<TDoc> aggregate, params string[] fields)
  {
    var set = new BsonDocument { { "$unset", new BsonArray(fields) } };
    return aggregate.AppendStage<TProj>(set);
  }

  #endregion

  #region Aggregate: $addFields 

  public static IAggregateFluent<TDoc> AddFields<TDoc>(this IAggregateFluent<TDoc> aggregate, BsonDocument fields)
      => AddFields<TDoc, TDoc>(aggregate, fields);

  public static IAggregateFluent<TProj> AddFields<TDoc, TProj>(this IAggregateFluent<TDoc> aggregate, BsonDocument fields)
  {
    var set = new BsonDocument { { "$addFields", fields } };
    return aggregate.AppendStage<TProj>(set);
  }

  #endregion

}