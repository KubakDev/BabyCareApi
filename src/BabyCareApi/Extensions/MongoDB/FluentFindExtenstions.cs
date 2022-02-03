using BabyCareApi.Abstractions;
using MongoDB.Driver;

namespace BabyCareApi.Extensions;
public static class FluentFindExtensions
{

  public static IFindFluent<TDoc, TProj> ApplySortOptions<TDoc, TProj>(this IFindFluent<TDoc, TProj> find, ISortOptions? sortOptions)
  {
    if (sortOptions is null || sortOptions.IsEmpty())
      return find;

    return find.Sort(sortOptions.BuildSortDefinition<TDoc>());
  }

  public static IFindFluent<TDoc, TProj> ApplyProjectionOptions<TDoc, TProj>(this IFindFluent<TDoc, TProj> find, IProjectionOptions? projectionOptions)
  {
    if (projectionOptions is null || projectionOptions.IsEmpty())
      return find;

    return find.Project<TProj>(projectionOptions.BuildProjectionDefinition<TDoc>());
  }

}

