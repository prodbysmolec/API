using Artikelsystem.Shared.DTOs.Artikel.Request;
using Artikelsystem.Shared.DTOs.Artikel.Enums;
using Domain.Entities.Artikel;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Artikelsystem.Shared.DTOs;
using Infrastructure.Common;
using AutoMapper;
using Application.Interfaces.Repositories;

namespace Infrastructure.Repositories;

public class ArtikelRepository(AppDbContext context) : GenericRepository<Artikel>(context), IArtikelRepository
{
    private readonly AppDbContext _context = context;

    public async Task<bool> AddArtikelAsync(Artikel artikel)
    {
        _context.Artikel.Add(artikel);
        return await Task.FromResult(true);
    }

    public async Task<PagedResultDTO<Artikel>> GetAllArtikelAsync(GetAllArtikelRequest request)
    {
        IQueryable<Artikel> artikel = _context.Artikel
            .Include(a => a.ArtikelStatistik)
            .Include(a => a.Artikelgruppe);

        // Filterung anwenden
        if (!string.IsNullOrEmpty(request.NameContains))
        {
            artikel = artikel.Where(a => a.Name.Contains(request.NameContains));
        }
        if (request.MinPreis.HasValue)
        {
            artikel = artikel.Where(a => a.Preis >= request.MinPreis.Value);
        }
        if (request.MaxPreis.HasValue)
        {
            artikel = artikel.Where(a => a.Preis <= request.MaxPreis.Value);
        }
        if (request.MinMenge.HasValue)
        {
            artikel = artikel.Where(a => a.Menge >= request.MinMenge.Value);
        }
        if (request.MaxMenge.HasValue)
        {
            artikel = artikel.Where(a => a.Menge <= request.MaxMenge.Value);
        }
        if (request.StatusId.HasValue)
        {
            artikel = artikel.Where(a => a.Status == (ArtikelStatus)request.StatusId.Value);
        }
        if (request.UnterMindestbestand.HasValue && request.UnterMindestbestand.Value)
        {
            artikel = artikel.Where(a => a.Menge < a.Mindestbestand);
        }
        if (request.UeberMaximalbestand.HasValue && request.UeberMaximalbestand.Value)
        {
            artikel = artikel.Where(a => a.Menge > a.Maximalbestand);
        }
        if (request.MinDurchschnittlicherEinzelpreis.HasValue)
        {
            artikel = artikel.Where(a => a.ArtikelStatistik!.DurchschnittlicherEinzelpreis >= request.MinDurchschnittlicherEinzelpreis.Value);
        }
        if (request.MaxDurchschnittlicherEinzelpreis.HasValue)
        {
            artikel = artikel.Where(a => a.ArtikelStatistik!.DurchschnittlicherEinzelpreis <= request.MaxDurchschnittlicherEinzelpreis.Value);
        }
        if (request.MinLagerwert.HasValue)
        {
            artikel = artikel.Where(a => a.ArtikelStatistik!.Lagerwert >= request.MinLagerwert.Value);
        }
        if (request.MaxLagerwert.HasValue)
        {
            artikel = artikel.Where(a => a.ArtikelStatistik!.Lagerwert <= request.MaxLagerwert.Value);
        }
        
        // Sortierung anwenden
        if (!string.IsNullOrEmpty(request.SortBy))
        {
            artikel = request.SortDesc.HasValue && request.SortDesc.Value
                ? artikel.OrderByDescending(e => EF.Property<object>(e, request.SortBy))
                : artikel.OrderBy(e => EF.Property<object>(e, request.SortBy));
        }

        // Validierung der Paging-Parameter
        int page = request.Page ?? 1;
        int recordsPerPage = request.RecordsPerPage ?? 10;

        if (page <= 0 || recordsPerPage <= 0)
        {
            throw new ArgumentException("Page and RecordsPerPage must be greater than zero.");
        }

        // Paging anwenden
        return await PagingService.ApplyPagingAsync(artikel, page, recordsPerPage);
    }

    public async Task<Artikel> GetArtikelByIdAsync(int id)
    {
        var artikel = await _context.Artikel
            .Include(a => a.ArtikelStatistik)
            .Include(a => a.Warenausgaenge)
                .ThenInclude(a => a.Warenausgang)
            .Include(a => a.ArtikelZusatzWerte)
                .ThenInclude(az => az.Zusatzwert)
            .Include(a => a.InventurHistorie)
            .Include(a => a.ArtikelLieferanten)
                .ThenInclude(a => a.Lieferant)
            .Include(a => a.Wareneingaenge)
                .ThenInclude(a => a.Wareneingang)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (artikel == null)
        {
            throw new ArgumentException($"Artikel mit ID {id} nicht gefunden.");
        }
        return artikel;
    }

    // public Task<GetArtikelResponse?> GetByIdAsync(int id, GetArtikelByIdRequest request)
    // {
    //     throw new NotImplementedException();
    // }

    // public Task<ArtikelStatistikDto?> GetStatistikAsync(int artikelId)
    // {
    //     throw new NotImplementedException();
    // }

    // public Task<IEnumerable<WarenausgangArtikelPositionenDto>> GetWarenausgaengeAsync(int artikelId)
    // {
    //     throw new NotImplementedException();
    // }

    // public Task<IEnumerable<WareneingangArtikelPositionenDto>> GetWareneingaengeAsync(int artikelId)
    // {
    //     throw new NotImplementedException();
    // }
}
