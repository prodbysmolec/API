using System;
using System.Linq.Expressions;
using Artikelsystem.Shared.DTOs;
namespace Application.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
    Task<T?> GetByIdAsync(object id);
    //Task<T?> GetByUserIdAsync(int userId, int id);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(object id);

    Task<PagedResultDTO<T>> GetAllFilteredAsync(
        Expression<Func<T, bool>>? filter = null,
        string? sortBy = null,
        bool sortDesc = false,
        int page = 1,
        int recordsPerPage = 10,
        CancellationToken cancellationToken = default);
}
