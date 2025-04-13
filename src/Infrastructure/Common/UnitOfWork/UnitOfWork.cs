using System;
using Application.Interfaces.Repositories;
using Application.Interfaces.UnitOfWork;
using Infrastructure.Context;
using Infrastructure.Repositories;

namespace Infrastructure.Common.UnitOfWork;

internal sealed class UnitOfWork(AppDbContext _context) : IUnitOfWork
{
    private readonly AppDbContext _context = _context;
    public IEmployeeRepository EmployeeRepository { get;} = new EmployeeRepository(_context); 

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RollbackAsync()
    {
        await _context.DisposeAsync();
    }
}
