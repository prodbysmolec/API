using System;
using Application.Interfaces.Repositories;
using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.Artikel.Request;
using Artikelsystem.Shared.DTOs.ArtikelGruppe.Request;
using Domain.Entities.Artikel;

namespace Application.Interfaces.Services;

public interface IArtikelGruppeService : IGenericRepository<Artikelgruppe>
{
    Task<bool> ExistsAsync(int id);
    Task<PagedResultDTO<GetAllArtikelGruppeResponse>> GetAllArtikelGruppen(int recordsPerPage, string? nameContains = null, int page = 1);
}
