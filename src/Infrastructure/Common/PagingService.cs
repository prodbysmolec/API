using System;
using Artikelsystem.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common;

public static class PagingService
{
    public static async Task<PagedResultDTO<T>> ApplyPagingAsync<T>(
        IQueryable<T> query,
        int page,
        int recordsPerPage) where T : class
    {
        int totalRecords = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * recordsPerPage)
            .Take(recordsPerPage)
            .ToListAsync();

        return new PagedResultDTO<T>
        {
            Items = items,
            Page = page,
            RecordsPerPage = recordsPerPage,
            TotalRecords = totalRecords
        };
    }
}
