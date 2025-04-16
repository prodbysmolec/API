using System;
using Application.Interfaces.Repositories;
using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.Artikel.Request;
using Artikelsystem.Shared.DTOs.ArtikelGruppe.Request;
using Domain.Entities.Artikel;

namespace Application.Interfaces.Repositories;

public interface IArtikelGruppeRepository : IGenericRepository<Artikelgruppe>
{
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<Artikelgruppe>> GetAllArtikelGruppen(string? nameContains = null);
}
