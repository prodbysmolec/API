using System.Linq.Expressions;
using Application.Interfaces.Repositories;
using Artikelsystem.Shared.DTOs;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public abstract class GenericRepository<T>(AppDbContext context)
    : IGenericRepository<T> where T : class
{
    public async Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        var entry = await context.Set<T>().AddAsync(entity, cancellationToken);
        return entry.State == EntityState.Added;
    }

    public Task<bool> DeleteAsync(T entity)
    {
        var entry = context.Set<T>().Remove(entity);
        return Task.FromResult(entry.State == EntityState.Deleted);
    }


    public async Task<PagedResultDTO<T>> GetAllFilteredAsync(
        Expression<Func<T, bool>>? filter = null,
        string? sortBy = null,
        bool sortDesc = false,
        int page = 1,
        int recordsPerPage = 10,
        CancellationToken cancellationToken = default)
    {
        var query = context.Set<T>().AsQueryable();

        // Apply filtering
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortDesc
                ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
                : query.OrderBy(e => EF.Property<object>(e, sortBy));
        }

        // Calculate total records
        var totalRecords = await query.CountAsync(cancellationToken);

        // Apply paging
        var items = await query
            .Skip((page - 1) * recordsPerPage)
            .Take(recordsPerPage)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new PagedResultDTO<T>
        {
            Items = items,
            TotalRecords = totalRecords
        };
    }


    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Set<T>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(object id)
    {
        return await context.Set<T>()
            .FindAsync(id)
            .AsTask();
    }

    public async Task<bool> ExistsAsync(object id)
    {
        return await context.Set<T>().FindAsync(id) != null;
    }
    public Task<bool> UpdateAsync(T entity)
    {
        var entry = context.Set<T>().Update(entity);
        return Task.FromResult(entry.State == EntityState.Modified);
    }
}
