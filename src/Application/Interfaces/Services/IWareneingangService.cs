using System;
using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Request;
using Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Response;

namespace Application.Interfaces.Services;

public interface IWareneingangService
{
    Task<List<GetWareneingaengeForArtikelResponse>> GetWareneingaengeForArtikelAsync(int artikelId);
    Task<int> AddWareneingangsPositionAsync(AddWareneingangsPositionRequest request);
    Task<PagedResultDTO<GetAlleWareneingaengeResponse>> GetAlleWareneingaengeAsync(int page, int recordsPerPage);
}
