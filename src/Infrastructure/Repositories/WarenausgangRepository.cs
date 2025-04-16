using Microsoft.EntityFrameworkCore;
using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Filter;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;
using Artikelsystem.Shared.DTOs.Artikel.Response;
using Artikelsystem.Shared.DTOs.Warenausgang.Enums;
using Domain.Entities.Warenausgang;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Request;
using API.Features.Inventur.Models.Enums;
using Infrastructure.Context;
using Application.Interfaces;

namespace Infrastructure.Repositories;

public class WarenausgangRepository : IWarenausgangRepository
{
    private readonly AppDbContext _context;
    public WarenausgangRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResultDTO<WarenausgangDto>> GetWarenausgaengeAsync(WarenausgangFilterDto filter, int pageNumber, int pageSize)
    {
        var query = _context.Warenausgaenge.AsQueryable();
        query = query.Include(w => w.ArtikelPositionen)
            .ThenInclude(ap => ap.Artikel);
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
                BearbeitetAm = w.BearbeitetAm,
                ArtikelPositionen = w.ArtikelPositionen.Select(ap => new WarenausgangArtikelPositionenDto
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
            })
            .ToListAsync();

        // Prüfe, ob die Liste leer ist
        items = items ?? new List<WarenausgangDto>();

        // Return paged result
        return new PagedResultDTO<WarenausgangDto>
        {
            TotalRecords = totalCount,
            Page = pageNumber,
            RecordsPerPage = pageSize,
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
    
    public async Task<WarenausgangDto> CreateWarenausgangAsync(WarenausgangRequestDto dto)
    {
        // Check if an active Inventur exists
        var activeInventur = await _context.Inventuren
            .Where(i => i.Status == InventurStatus.InBearbeitung).ToListAsync();
        var activeInventurExists = activeInventur.Any();
        if (activeInventurExists)
        {
            throw new InvalidOperationException("Ein Warenausgang kann nicht gebucht werden, während eine Inventur aktiv ist.");
        }

        // Validierung des Zwecks
        if (!Enum.IsDefined(typeof(WarenausgangZweckEnum), dto.Zweck))
        {
            throw new ArgumentException("Ungültiger Zweck für den Warenausgang.");
        }
        if (dto.Zweck == WarenausgangZweckEnum.None)
        {
            throw new ArgumentException("Der Zweck des Warenausgangs darf nicht 'None' sein.");
        }

        // Warenausgang erstellen
        var warenausgang = new Warenausgaenge
        {
            Zweck = dto.Zweck,
            AllgemeineBemerkungen = dto.AllgemeineBemerkungen,
            ErstelltAm = DateTime.UtcNow,
        };

        _context.Warenausgaenge.Add(warenausgang);
        await _context.SaveChangesAsync();

        // Artikelpositionen hinzufügen
        foreach (var position in dto.ArtikelPositionen)
        {
            var artikel = await _context.Artikel.FindAsync(position.ArtikelId);
            if (artikel == null)
            {
                throw new ArgumentException($"Artikel mit ID {position.ArtikelId} nicht gefunden.");
            }

            var artikelPosition = new WarenausgangArtikelPositionen
            {
                WarenausgangId = warenausgang.Id,
                ArtikelId = position.ArtikelId,
                Menge = position.Menge,
                Bemerkung = position.Bemerkung,
                Verkaufspreis = position.Verkaufspreis,
                Rechnungsnummer = position.Rechnungsnummer,
                Gesamtpreis = position.Menge * (position.Verkaufspreis ?? artikel.Preis)
            };

            _context.WarenausgangArtikelPosition.Add(artikelPosition);
        }

        await _context.SaveChangesAsync();

        // Map to DTO
        var warenausgangDto = new WarenausgangDto
        {
            Id = warenausgang.Id,
            Zweck = warenausgang.Zweck,
            AllgemeineBemerkungen = warenausgang.AllgemeineBemerkungen,
            ErstelltAm = warenausgang.ErstelltAm,
            ErstelltVon = warenausgang.ErstelltVon,
            ArtikelPositionen = warenausgang.ArtikelPositionen.Select(ap => new WarenausgangArtikelPositionenDto
            {
                Id = ap.Id,
                WarenausgangId = ap.WarenausgangId,
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

        return warenausgangDto;
    }

    public async Task<List<WarenausgangArtikelPositionenDto>> GetWarenausgangArtikelPositionenByWarenausgangIdAsync(int warenausgangId)
    {
        var warenausgangArtikelPositionen = await _context.WarenausgangArtikelPosition
            .Where(w => w.WarenausgangId == warenausgangId)
            .Include(w => w.Artikel)
            .ThenInclude(a => a.ArtikelStatistik)
            .ToListAsync();

        if (warenausgangArtikelPositionen == null || !warenausgangArtikelPositionen.Any())
        {
            return new List<WarenausgangArtikelPositionenDto>();
        }

        return warenausgangArtikelPositionen.Select(wap => new WarenausgangArtikelPositionenDto
        {
            Id = wap.Id,
            WarenausgangId = wap.WarenausgangId,
            ArtikelId = wap.ArtikelId,
            ArtikelName = wap.Artikel.Name,
            Menge = wap.Menge,
            Bemerkung = wap.Bemerkung,
            Verkaufspreis = wap.Verkaufspreis,
            Rechnungsnummer = wap.Rechnungsnummer,
            Gesamtpreis = wap.Gesamtpreis,
            Artikel = new ArtikelDto
            {
                Id = wap.Artikel.Id,
                Name = wap.Artikel.Name,
                Maximalbestand = wap.Artikel.Maximalbestand,
                Mindestbestand = wap.Artikel.Mindestbestand,
                Preis = wap.Artikel.Preis,
                Menge = wap.Artikel.Menge,
                Status = wap.Artikel.Status,
                Bild = wap.Artikel.Bild,
                ArtikelStatistik = wap.Artikel.ArtikelStatistik != null
                    ? new ArtikelStatistikDto
                    {
                        Id = wap.Artikel.ArtikelStatistik.Id,
                        ArtikelId = wap.Artikel.ArtikelStatistik.ArtikelId,
                        Gesamtmenge = wap.Artikel.ArtikelStatistik.Gesamtmenge,
                        DurchschnittlicherEinzelpreis = wap.Artikel.ArtikelStatistik.DurchschnittlicherEinzelpreis,
                        DurchschnittlicherVerkaufspreis = wap.Artikel.ArtikelStatistik.DurchschnittlicherVerkaufspreis,
                        VerkaufsMenge = wap.Artikel.ArtikelStatistik.VerkaufsMenge,
                        Lagerwert = wap.Artikel.ArtikelStatistik.Lagerwert,
                        GesamtVerkaufswert = wap.Artikel.ArtikelStatistik.GesamtVerkaufswert
                    }
                    : null
            }
        }).ToList();
    }

    public async Task<bool> CanEditWarenausgangAsync(int warenausgangId, string userId, bool isAdmin)
    {
        var warenausgang = await _context.Warenausgaenge.FindAsync(warenausgangId);
        if (warenausgang == null)
            return false;

        return isAdmin || warenausgang.ErstelltVon == userId;
    }

    public async Task UpdateWarenausgangAsync(int id, WarenausgangRequestDto dto, string userId, bool isAdmin)
    {
        var warenausgang = await _context.Warenausgaenge
            .Include(w => w.ArtikelPositionen)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (warenausgang == null)
            throw new ArgumentException("Warenausgang nicht gefunden.");

        if (!await CanEditWarenausgangAsync(id, userId, isAdmin))
            throw new UnauthorizedAccessException("Keine Berechtigung, diesen Warenausgang zu bearbeiten.");

        // Aktualisiere die Felder
        warenausgang.Zweck = dto.Zweck;
        warenausgang.AllgemeineBemerkungen = dto.AllgemeineBemerkungen;
        warenausgang.BearbeitetAm = DateTime.UtcNow;
        warenausgang.BearbeitetVon = userId;

        // Aktualisiere Artikelpositionen (optional, falls benötigt)
        // ... Logik für Artikelpositionen ...

        await _context.SaveChangesAsync();
    }

    public Task<bool> DeleteWarenausgangAsync(int id)
    {
        var warenausgang = _context.Warenausgaenge.Find(id);
        if (warenausgang == null)
            return Task.FromResult(false);

        _context.Warenausgaenge.Remove(warenausgang);
        return _context.SaveChangesAsync().ContinueWith(t => t.Result > 0);
    }
}
