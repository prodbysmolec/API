using Artikelsystem.Api.Features.Artikel.Repositories;
using Artikelsystem.Api.Features.Lieferant.Repositories;
using Artikelsystem.Api.Features.Wareneingang.Repositories;

namespace Artikelsystem.Api.Infrastructure.Persistence.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IArtikelRepository ArtikelRepository { get; }
    IArtikelStatistikRepository ArtikelStatistikRepository { get; }
    ILieferantRepository LieferantRepository { get; }
    IWareneingangRepository WareneingangRepository { get; }
    IWareneingangArtikelRepository WareneingangArtikelRepository { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}