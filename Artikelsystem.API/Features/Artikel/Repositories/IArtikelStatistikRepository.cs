using Artikelsystem.Api.Features.Artikel.Models.Entitys;
using Artikelsystem.Api.Infrastructure.Persistence.Repositories;

namespace Artikelsystem.Api.Features.Artikel.Repositories;

public interface IArtikelStatistikRepository : IRepository<ArtikelStatistik>
{
    Task<bool> AktualisiereStatistikNachWareneingangAsync(int artikelId);
}