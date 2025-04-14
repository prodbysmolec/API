using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artikelsystem.Shared.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common;

public static class PagingService
{
    public static async Task<PagedResultDTO<T>> ApplyPagingAsync<T>(
        IQueryable<T> query,
        int page = 1,
        int recordsPerPage = 10) where T : class
    {
        if (page <= 0 || recordsPerPage <= 0)
        {
            throw new ArgumentException("Page and RecordsPerPage must be greater than zero.");
        }

        int totalRecords = await query.CountAsync();

        if(page > (int)Math.Ceiling((double)totalRecords / recordsPerPage))
        {
            throw new ArgumentException("Die eingegebenen Seitenanzahl ist größer als die Gesamtseitenanzahl.");
        }

        var items = await query
            .Skip((page - 1) * recordsPerPage)
            .Take(recordsPerPage)
            .ToListAsync();

        return new PagedResultDTO<T>
        {
            Items = items,
            Page = page,
            RecordsPerPage = recordsPerPage,
            TotalRecords = totalRecords,
        };
    }

    public static async Task<PagedResultDTO<TDestination>> GetPagedAndMappedResultAsync<TSource, TDestination>(
        IQueryable<TSource> query,
        IMapper mapper,
        int page = 1,
        int recordsPerPage = 10) where TSource : class where TDestination : class
    {
        if (page <= 0 || recordsPerPage <= 0)
        {
            throw new ArgumentException("Die Seite und die angezeigten Records pro Seite müssen größer als 0 sein.");
        }


        // Paging anwenden
        var pagedResult = await ApplyPagingAsync(query, page, recordsPerPage);

        // Überprüfen, ob Ergebnisse vorhanden sind
        if (pagedResult == null || !pagedResult.Items.Any())
        {
            return new PagedResultDTO<TDestination>
            {
                Items = new List<TDestination>(),
                Page = page,
                RecordsPerPage = recordsPerPage,
                TotalRecords = 0
            };
        }

        // Mapping der Ergebnisse
        var mappedResult = mapper.Map<IEnumerable<TDestination>>(pagedResult.Items);

        // Ergebnis in ein PagedResultDTO verpacken
        return new PagedResultDTO<TDestination>
        {
            Items = mappedResult.ToList(),
            TotalRecords = pagedResult.TotalRecords,
            RecordsPerPage = recordsPerPage,
            Page = page
        };
    }
}