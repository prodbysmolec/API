using System;
using Application.Queries.Artikel;
using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.Artikel.Request;
using Artikelsystem.Shared.DTOs.Artikel.Response;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;
using Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Response;
using Domain.Entities.Artikel;
using Domain.Entities.Employees;

namespace Application.Interfaces.Services;

public interface IArtikelService
{

    Task<PagedResultDTO<Artikel>> GetAllArtikelAsync(
        GetAllArtikelRequest request
    );
    Task<int> AddArtikelAsync(Artikel artikel);
    Task<Artikel> GetArtikelByIdAsync(int id);

    // Task<GetArtikelResponse?> GetByIdAsync(int id, GetArtikelByIdRequest request);
    // Task<IEnumerable<WareneingangArtikelPositionenDto>> GetWareneingaengeAsync(int artikelId);
    // Task<IEnumerable<WarenausgangArtikelPositionenDto>> GetWarenausgaengeAsync(int artikelId);
    // Task<ArtikelStatistikDto?> GetStatistikAsync(int artikelId);
}
