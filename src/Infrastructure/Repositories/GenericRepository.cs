using System;
using Application.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal abstract class GenericRepository<T>(AppDbContext context)
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

    // public async Task<T?> GetByUserIdAsync(Guid userId, Guid id)
    // {
    //     return await context.Set<T>()
    //         .AsNoTracking()
    //         .FirstOrDefaultAsync(e => e.UserId == userId && e.Id == id);
    // }
    public Task<bool> UpdateAsync(T entity)
    {
        var entry = context.Set<T>().Update(entity);
        return Task.FromResult(entry.State == EntityState.Modified);
    }
}
