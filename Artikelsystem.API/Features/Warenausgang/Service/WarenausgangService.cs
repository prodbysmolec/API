using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Artikelsystem.Api.Infrastructure.Persistence.Context;
using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Filter;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;
using Artikelsystem.Shared.DTOs.Artikel.Response;

namespace Artikelsystem.API.Features.Warenausgang.Service;

public class WarenausgangService : IWarenausgangService
{
    private readonly AppDbContext _context;
    public WarenausgangService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResultDTO<WarenausgangDto>> GetWarenausgaengeAsync(WarenausgangFilterDto filter, int pageNumber, int pageSize)
    {
        var query = _context.Warenausgaenge.AsQueryable();

        // Apply filters
        if (filter.VonDatum.HasValue)
            query = query.Where(w => w.ErstelltAm >= filter.VonDatum.Value);

        if (filter.BisDatum.HasValue)
            query = query.Where(w => w.ErstelltAm <= filter.BisDatum.Value);

        if (!string.IsNullOrEmpty(filter.ErstelltVon))
            query = query.Where(w => w.ErstelltVon == filter.ErstelltVon);

        if (!string.IsNullOrEmpty(filter.GeaendertVon))
            query = query.Where(w => w.BearbeitetVon == filter.GeaendertVon);

        // Apply sorting
        if (!string.IsNullOrEmpty(filter.SortBy))
        {
            query = filter.SortDescending == true
                ? query.OrderByDescending(e => EF.Property<object>(e, filter.SortBy))
                : query.OrderBy(e => EF.Property<object>(e, filter.SortBy));
        }

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(w => new WarenausgangDto
            {
                Id = w.Id,
                Zweck = w.Zweck,
                AllgemeineBemerkungen = w.AllgemeineBemerkungen,
                ErstelltAm = w.ErstelltAm,
                ErstelltVon = w.ErstelltVon,
                BearbeitetVon = w.BearbeitetVon,
                BearbeitetAm = w.BearbeitetAm
            })
            .ToListAsync();

        // Prüfe, ob die Liste leer ist
        items = items ?? new List<WarenausgangDto>();

        // Return paged result
        return new PagedResultDTO<WarenausgangDto>
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Items = items
        };
    }

    public async Task<WarenausgangDto?> GetWarenausgangByIdAsync(int id)
    {
        var warenausgang = await _context.Warenausgaenge
            .Include(w => w.ArtikelPositionen)
            .ThenInclude(ap => ap.Artikel)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (warenausgang == null)
        {
            return null;
        }

        return new WarenausgangDto
        {
            Id = warenausgang.Id,
            Zweck = warenausgang.Zweck,
            AllgemeineBemerkungen = warenausgang.AllgemeineBemerkungen,
            ErstelltAm = warenausgang.ErstelltAm,
            ErstelltVon = warenausgang.ErstelltVon,
            BearbeitetVon = warenausgang.BearbeitetVon,
            BearbeitetAm = warenausgang.BearbeitetAm,
            ArtikelPositionen = warenausgang.ArtikelPositionen.Select(ap => new WarenausgangArtikelPositionenDto
            {
                Id = ap.Id,
                WarenausgangId = ap.Warenausgang.Id,
                ArtikelId = ap.ArtikelId,
                ArtikelName = ap.Artikel.Name,
                Menge = ap.Menge,
                Bemerkung = ap.Bemerkung,
                Verkaufspreis = ap.Verkaufspreis,
                Rechnungsnummer = ap.Rechnungsnummer,
                Gesamtpreis = ap.Gesamtpreis,
                Artikel = new ArtikelDto 
                {
                    Id = ap.Artikel.Id,
                    Name = ap.Artikel.Name,
                    Maximalbestand = ap.Artikel.Maximalbestand,
                    Mindestbestand = ap.Artikel.Mindestbestand,
                    Preis = ap.Artikel.Preis,
                    Menge = ap.Artikel.Menge,
                    Status = ap.Artikel.Status,
                    Bild = ap.Artikel.Bild
                }
            }).ToList()
        };
    }
}
