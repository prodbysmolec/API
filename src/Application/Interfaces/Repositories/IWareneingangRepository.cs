using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Request;
using Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Response;
using Domain.Entities.Wareneingang;

namespace Application.Interfaces.Repositories;

public interface IWareneingangRepository : IGenericRepository<Wareneingaenge>
{
    Task<int> AddWareneingangsPositionAsync(AddWareneingangsPositionRequest request);
    Task<PagedResultDTO<GetAlleWareneingaengeResponse>> GetAlleWareneingaengeAsync(int page, int recordsPerPage);
    // Task<List<GetWareneingaengeForArtikelResponse>> GetWareneingaengeForArtikelAsync(int artikelId);
}
