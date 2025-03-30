using Artikelsystem.Api.Features.Wareneingang.Models.Entitys;
using Artikelsystem.Api.Infrastructure.Persistence.Repositories;

namespace Artikelsystem.Api.Features.Wareneingang.Repositories;

public interface IWareneingangRepository : IRepository<Models.Entitys.Wareneingang>
{
    Task<Models.Entitys.Wareneingang?> GetWareneingangMitArtikelAsync(int id);
    Task<IEnumerable<Models.Entitys.Wareneingang>> GetWareneingaengeByLieferantAsync(int lieferantId);
}