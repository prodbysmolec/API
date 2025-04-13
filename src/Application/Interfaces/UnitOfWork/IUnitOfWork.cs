using System;
using Application.Interfaces.Repositories;

namespace Application.Interfaces.UnitOfWork;

public interface IUnitOfWork
{
    IEmployeeRepository EmployeeRepository { get; }
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync();
}
