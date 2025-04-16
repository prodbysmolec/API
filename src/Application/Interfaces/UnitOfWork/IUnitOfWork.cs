using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;

namespace Application.Interfaces.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    // Repository-Properties
    IArtikelGruppeRepository ArtikelGruppeRepository { get; }
    IArtikelRepository ArtikelRepository { get; }
    IEmployeeRepository EmployeeRepository { get; }
    IPermissionRepository PermissionRepository { get; }
    IUserGruppenRepository UserGruppenRepository { get; }
    IUserRepository UserRepository { get; }
    IWareneingangRepository WareneingangRepository { get; }
    IWarenausgangRepository WarenausgangRepository { get; }
    
    // Generic Repository-Methode
    IGenericRepository<T> GetRepository<T>() where T : class;
    
    // Transaktionsmanagement
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    
    // Persistenz
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
    
    // Legacy-Methode für Abwärtskompatibilität (optional)
    Task RollbackAsync();
}