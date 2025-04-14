using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Helfer;

public static class QueryHelper
{
    // Anwenden von Filtern
    public static IQueryable<T> ApplyFilter<T>(
        IQueryable<T> query,
        Expression<Func<T, bool>>? filter
    ) where T : class
    {
        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query;
    }

    // Anwenden von Sortierung
    public static IQueryable<T> ApplySorting<T>(
        IQueryable<T> query,
        string? sortBy,
        bool? sortDesc
    ) where T : class
    {
        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortDesc == true
                ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
                : query.OrderBy(e => EF.Property<object>(e, sortBy));
        }

        return query;
    }

    // Anwenden von Pagination
    public static IQueryable<T> ApplyPagination<T>(
        IQueryable<T> query,
        int page,
        int recordsPerPage
    ) where T : class
    {
        return query
            .Skip((page - 1) * recordsPerPage)
            .Take(recordsPerPage);
    }
}