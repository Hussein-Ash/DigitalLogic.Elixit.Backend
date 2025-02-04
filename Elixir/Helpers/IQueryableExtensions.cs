using Elixir.DATA.DTOs;

namespace Elixir.Helpers;
public static class IQueryableExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, BaseFilter paginationFilter)
    {
        return query
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize);
    }
}