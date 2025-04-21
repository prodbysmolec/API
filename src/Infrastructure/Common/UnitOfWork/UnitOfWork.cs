using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.UnitOfWork;

public sealed class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly AppDbContext _context = null!;
    private readonly ICurrentUserService _currentUserService;
    private IDbContextTransaction? _transaction; // Nullable gemacht
    private bool _disposed;

    // Repository Properties
    public IEmployeeRepository EmployeeRepository { get; }
    public IArtikelRepository ArtikelRepository { get; }
    public IArtikelGruppeRepository ArtikelGruppeRepository { get; }
    public IUserGruppenRepository UserGruppenRepository { get; }
    public IWareneingangRepository WareneingangRepository { get; }
    public IWarenausgangRepository WarenausgangRepository { get; }
    public IPermissionRepository PermissionRepository { get; }
    public IUserRepository UserRepository { get; }

    public UnitOfWork(
        AppDbContext context,
        ICurrentUserService currentUserService,
        IEmployeeRepository employeeRepository,
        IArtikelRepository artikelRepository,
        IArtikelGruppeRepository artikelGruppeRepository,
        IUserGruppenRepository userGruppenRepository,
        IWareneingangRepository wareneingangRepository,
        IWarenausgangRepository warenausgangRepository,
        IPermissionRepository permissionRepository,
        IUserRepository userRepository)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));

        // Verwende die injizierten Repositories
        EmployeeRepository = employeeRepository;
        ArtikelRepository = artikelRepository;
        ArtikelGruppeRepository = artikelGruppeRepository;
        UserGruppenRepository = userGruppenRepository;
        WareneingangRepository = wareneingangRepository;
        WarenausgangRepository = warenausgangRepository;
        PermissionRepository = permissionRepository;
        UserRepository = userRepository;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        return _transaction;
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("Transaktion wurde nicht gestartet");
        }

        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("Transaktion wurde nicht gestartet");
        }

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        // Audit-Informationen für alle geänderten Entities setzen
        ApplyAuditInfo();
        return await _context.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInfo()
    {
        var entries = _context.ChangeTracker.Entries();
        var now = DateTime.UtcNow;
        var currentUser = _currentUserService.UserName ?? "prodbysmolec";

        foreach (var entry in entries)
        {
            // Korrekter Cast zu AuditableEntity statt IAuditable
            if (entry.Entity is AuditableEntity auditableEntity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        auditableEntity.ErstelltAm = now;
                        auditableEntity.ErstelltVon = currentUser;
                        break;

                    case EntityState.Modified:
                        auditableEntity.BearbeitetAm = now;
                        auditableEntity.BearbeitetVon = currentUser;
                        break;
                }
            }
        }
    }

    public async Task RollbackAsync()
    {
        // Wenn eine Transaktion aktiv ist, rollen wir diese zurück
        if (_transaction != null)
        {
            await RollbackTransactionAsync();
        }
        else
        {
            // Sonst nur den DbContext zurücksetzen
            await _context.DisposeAsync();
        }
    }

    public IGenericRepository<T> GetRepository<T>() where T : class
    {
        throw new NotImplementedException("Generische Repository-Erstellung noch nicht implementiert");
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
        _disposed = true;
    }
}