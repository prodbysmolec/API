using Artikelsystem.Api.Features.Artikel.Repositories;
using Artikelsystem.Api.Features.Lieferant.Repositories;
using Artikelsystem.Api.Features.Wareneingang.Repositories;
using Artikelsystem.Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Artikelsystem.Api.Infrastructure.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    private IDbContextTransaction? _transaction;
    private bool _disposed = false;

    public IArtikelRepository ArtikelRepository { get; }
    public IArtikelStatistikRepository ArtikelStatistikRepository { get; }
    public ILieferantRepository LieferantRepository { get; }
    public IWareneingangRepository WareneingangRepository { get; }
    public IWareneingangArtikelRepository WareneingangArtikelRepository { get; }

    public UnitOfWork(
        AppDbContext dbContext,
        IArtikelRepository artikelRepository,
        IArtikelStatistikRepository artikelStatistikRepository,
        ILieferantRepository lieferantRepository,
        IWareneingangRepository wareneingangRepository,
        IWareneingangArtikelRepository wareneingangArtikelRepository)
    {
        _dbContext = dbContext;
        ArtikelRepository = artikelRepository;
        ArtikelStatistikRepository = artikelStatistikRepository;
        LieferantRepository = lieferantRepository;
        WareneingangRepository = wareneingangRepository;
        WareneingangArtikelRepository = wareneingangArtikelRepository;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
                _transaction?.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}