using Artikelsystem.Api.Features.Wareneingang.Models.Entitys;
using Artikelsystem.Api.Infrastructure.Persistence.Repositories;

namespace Artikelsystem.Api.Features.Wareneingang.Repositories;

public interface IWareneingangArtikelRepository : IRepository<WareneingangArtikel>
{
    Task<IEnumerable<WareneingangArtikel>> GetByWareneingangIdAsync(int wareneingangId);
    Task<IEnumerable<WareneingangArtikel>> GetByArtikelIdAsync(int artikelId);
}