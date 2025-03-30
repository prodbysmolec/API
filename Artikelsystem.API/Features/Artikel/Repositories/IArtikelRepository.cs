using Artikelsystem.Api.Features.Artikel.Models.Entitys;
using Artikelsystem.Api.Infrastructure.Persistence.Repositories;

namespace Artikelsystem.Api.Features.Artikel.Repositories;

public interface IArtikelRepository : IRepository<Models.Entitys.Artikel>
{
    Task<Models.Entitys.Artikel?> GetArtikelMitStatistikAsync(int id);
    Task<IEnumerable<Models.Entitys.Artikel>> GetArtikelMitBestandUnterMindestbestandAsync();
    Task<bool> UpdateArtikelBestandAsync(int artikelId, int menge);
}