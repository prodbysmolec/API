using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artikelsystem.Api.Features.Artikel.Models.Entitys;
using Artikelsystem.Api.Features.Inventur.Models.Dtos;
using Artikelsystem.Api.Features.Inventur.Models.Entitys;
using Artikelsystem.Api.Features.Inventur.Models.Enums;
using Artikelsystem.Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Artikelsystem.Api.Features.Inventur.Services;


public class InventurService : IInventurService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<InventurService> _logger;

    public InventurService(AppDbContext dbContext, ILogger<InventurService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<InventurDto> ErstelleInventur(CreateInventurRequest request)
    {
        var inventur = new Models.Entitys.Inventur
        {
            Bezeichnung = request.Bezeichnung,
            StartDatum = DateTime.UtcNow,
            Status = InventurStatus.Erstellt,
            Bemerkung = request.Bemerkung,
            CreatedBy = request.ErstelltVon,
            CreatedOn = DateTime.UtcNow,
            LastModifiedBy = request.ErstelltVon,
            LastModifiedOn = DateTime.UtcNow
        };

        _dbContext.Inventuren.Add(inventur);
        await _dbContext.SaveChangesAsync();

        return MapToInventurDto(inventur);
    }

    public async Task<InventurDto> StarteInventur(int inventurId)
    {
        var existierendeInventur = await _dbContext.Inventuren
            .AnyAsync(i => i.Status == InventurStatus.InBearbeitung);

        if (existierendeInventur)
        {
            throw new InvalidOperationException($"Inventur kann nicht gestartet werden, da eine andere Inventur aktuell noch läuft.");
        }

        var inventur = await _dbContext.Inventuren
            .Include(i => i.Positionen)
                .ThenInclude(p => p.Artikel)  // Make sure to include the Artikel entity
            .FirstOrDefaultAsync(i => i.Id == inventurId);

        if (inventur == null)
        {
            throw new KeyNotFoundException($"Inventur mit ID {inventurId} nicht gefunden");
        }

        if (inventur.Status != InventurStatus.Erstellt)
        {
            throw new InvalidOperationException($"Inventur kann nicht gestartet werden, aktueller Status: {inventur.Status}");
        }

        // Status aktualisieren
        inventur.Status = InventurStatus.InBearbeitung;
        inventur.LastModifiedOn = DateTime.UtcNow;

        // IDs der existierenden Positionen abrufen
        var existierendeArtikelIds = new HashSet<int>(await _dbContext.InventurPositionen
            .Where(p => p.InventurId == inventurId)
            .Select(p => p.ArtikelId)
            .ToListAsync());

        // Neue Artikel abrufen, die noch nicht in der Inventur vorhanden sind
        var alleArtikel = await _dbContext.Artikel.ToListAsync(); // Get all articles first
        
        var neueArtikelPositionen = new List<InventurPosition>();
        
        foreach (var artikel in alleArtikel)
        {
            if (!existierendeArtikelIds.Contains(artikel.Id))
            {
                var neuePosition = new InventurPosition
                {
                    InventurId = inventurId,
                    ArtikelId = artikel.Id,
                    Artikel = artikel, // Set the full Artikel entity
                    Menge = artikel.Menge,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = inventur.LastModifiedBy,
                    LastModifiedOn = DateTime.UtcNow,
                    LastModifiedBy = inventur.LastModifiedBy
                };
                
                neueArtikelPositionen.Add(neuePosition);
            }
        }

        if (neueArtikelPositionen.Any())
        {
            await _dbContext.InventurPositionen.AddRangeAsync(neueArtikelPositionen);
            
            // Make sure the Positionen collection is initialized
            if (inventur.Positionen == null)
            {
                inventur.Positionen = new List<InventurPosition>();
            }
            
            // Add new articles to the inventur.Positionen collection
            foreach (var position in neueArtikelPositionen)
            {
                inventur.Positionen.Add(position);
            }
        }

        _dbContext.Entry(inventur).State = EntityState.Modified;

        await _dbContext.SaveChangesAsync();
        
        // Reload the inventory with all related data to ensure proper mapping
        inventur = await _dbContext.Inventuren
            .Include(i => i.Positionen)
                .ThenInclude(p => p.Artikel)
            .FirstOrDefaultAsync(i => i.Id == inventurId);

        return MapToInventurDto(inventur!);
    }

    public async Task<InventurDto> GetInventurById(int inventurId)
    {
        var inventur = await _dbContext.Inventuren
            .Include(i => i.Positionen)
            .ThenInclude(p => p.Artikel)
            .FirstOrDefaultAsync(i => i.Id == inventurId);

        if (inventur == null)
        {
            throw new KeyNotFoundException($"Inventur mit ID {inventurId} nicht gefunden");
        }

        return MapToInventurDto(inventur);
    }

    public async Task<List<InventurDto>> GetAlleInventuren()
    {
        var inventuren = await _dbContext.Inventuren
            .Include(i => i.Positionen)
            .ToListAsync();

        return inventuren.Select(MapToInventurDto).ToList();
    }

    public async Task<InventurPositionDto> AktualisieereInventurPosition(UpdateInventurPositionRequest request)
    {
        var position = await _dbContext.InventurPositionen
            .Include(p => p.Artikel)
            .FirstOrDefaultAsync(p => p.Id == request.PositionId && p.InventurId == request.InventurID && p.ArtikelId == request.ArtikelId);

        if (position == null)
        {
            throw new KeyNotFoundException($"Inventurposition mit ID {request.PositionId} nicht gefunden");
        }

        // Inventur darf nur im Status "InBearbeitung" bearbeitet werden
        var inventur = await _dbContext.Inventuren.FindAsync(position.InventurId);
        if (inventur != null && inventur.Status != InventurStatus.InBearbeitung)
        {
            throw new InvalidOperationException($"Inventur kann nicht bearbeitet werden, aktueller Status: {inventur.Status}");
        }


        position.GezaehlteMenge = request.GezaehlteMenge;
        position.IstGeprueft = request.IstGeprueft;
        position.Bemerkung = request.Bemerkung;
        position.LastModifiedOn = DateTime.UtcNow;
        position.LastModifiedBy = request.BearbeitetVon;
        
        // Differenzwert berechnen
        if (position.GezaehlteMenge.HasValue && position.Artikel != null)
        {
            var differenz = position.GezaehlteMenge.Value - position.Menge;
            if(position.GezaehlteMenge.Value != 0)
            {
            position.DifferenzWert = differenz * position.Artikel.Preis;
            }
        }

        var isTracked = _dbContext.ChangeTracker.Entries<InventurPosition>()
        .Any(e => e.Entity.Id == position.Id && e.State != EntityState.Unchanged);
        Console.WriteLine($"Wird getrackt: {isTracked}");
        _dbContext.Entry(position).State = EntityState.Modified;

        await _dbContext.SaveChangesAsync();

        return MapToInventurPositionDto(position);
    }


    public async Task<List<InventurBerichtDto>> GetInventurBerichte()
    {
        var berichte = await _dbContext.InventurBerichte
            .Include(b => b.Inventur)
            .OrderByDescending(b => b.Erstellungsdatum)
            .ToListAsync();
            
        return berichte.Select(MapToInventurBerichtDto).ToList();
    }

    public async Task<InventurBerichtDto> GetInventurBerichtById(int berichtId)
    {
        var bericht = await _dbContext.InventurBerichte
            .Include(b => b.Inventur)
            .FirstOrDefaultAsync(b => b.Id == berichtId);
            
        if (bericht == null)
        {
            throw new KeyNotFoundException($"Inventurbericht mit ID {berichtId} nicht gefunden");
        }
        
        return MapToInventurBerichtDto(bericht);
    }

    public async Task<InventurBerichtDto?> GetInventurBerichtFuerInventur(int inventurId)
    {
        var bericht = await _dbContext.InventurBerichte
            .Include(b => b.Inventur)
            .FirstOrDefaultAsync(b => b.InventurId == inventurId);
            
        if (bericht == null)
        {
            return null;
        }
        
        return MapToInventurBerichtDto(bericht);
    }

    public async Task<InventurBerichtDto> GenerateInventurBericht(int inventurId, string benutzer)
    {
        var inventur = await _dbContext.Inventuren
            .Include(i => i.Positionen)
            .ThenInclude(p => p.Artikel)
            .FirstOrDefaultAsync(i => i.Id == inventurId);
        
        if (inventur == null)
        {
            throw new KeyNotFoundException($"Inventur mit ID {inventurId} nicht gefunden");
        }
        
        // Berichtsdaten sammeln
        var positionenMitDifferenz = inventur.Positionen
            .Where(p => p.GezaehlteMenge.HasValue && p.GezaehlteMenge.Value != p.Menge)
            .ToList();
            
        var gesamtDifferenzWert = positionenMitDifferenz
            .Sum(p => p.DifferenzWert ?? 0);
        
        // Berichtsinhalt erstellen (als formatierter Text)
        var berichtsInhalt = new System.Text.StringBuilder();
        berichtsInhalt.AppendLine($"# Inventurbericht: {inventur.Bezeichnung}");
        berichtsInhalt.AppendLine($"Datum: {DateTime.UtcNow:dd.MM.yyyy HH:mm:ss}");
        berichtsInhalt.AppendLine($"Status: {inventur.Status}");
        berichtsInhalt.AppendLine($"Erstellt von: {benutzer}");
        berichtsInhalt.AppendLine();
        berichtsInhalt.AppendLine($"## Zusammenfassung");
        berichtsInhalt.AppendLine($"Gesamtzahl der Positionen: {inventur.Positionen.Count}");
        berichtsInhalt.AppendLine($"Positionen mit Differenzen: {positionenMitDifferenz.Count}");
        berichtsInhalt.AppendLine($"Gesamtdifferenzwert: {gesamtDifferenzWert:C2}");
        berichtsInhalt.AppendLine();
        
        // Detaillierte Liste der Differenzen
        if (positionenMitDifferenz.Any())
        {
            berichtsInhalt.AppendLine("## Positionen mit Differenzen");
            berichtsInhalt.AppendLine("| Artikel | Systemmenge | Gezählte Menge | Differenz | Differenzwert | Bemerkung |");
            berichtsInhalt.AppendLine("|---------|-------------|---------------|-----------|---------------|-----------|");
            
            foreach (var position in positionenMitDifferenz.OrderByDescending(p => p.DifferenzWert))
            {
                berichtsInhalt.AppendLine($"| {position.Artikel.Name} | {position.Menge} | {position.GezaehlteMenge} | {position.Differenz} | {position.DifferenzWert:C2} | {position.Bemerkung} |");
            }
        }
        
        // Bericht in der Datenbank speichern
        var bericht = new InventurBerichte
        {
            InventurId = inventurId,
            Titel = $"Inventurbericht: {inventur.Bezeichnung}",
            Inhalt = berichtsInhalt.ToString(),
            Erstellungsdatum = DateTime.UtcNow,
            GesamtDifferenzWert = gesamtDifferenzWert,
            AnzahlPositionenMitDifferenz = positionenMitDifferenz.Count,
            CreatedBy = benutzer,
            CreatedOn = DateTime.UtcNow,
            LastModifiedBy = benutzer,
            LastModifiedOn = DateTime.UtcNow
        };
        
        _dbContext.InventurBerichte.Add(bericht);
        await _dbContext.SaveChangesAsync();
        
        return MapToInventurBerichtDto(bericht);
    }



    // Hilfsmethoden für das Mapping
private InventurDto MapToInventurDto(Models.Entitys.Inventur inventur)
{
    var dto = new InventurDto
    {
        Id = inventur.Id,
        Bezeichnung = inventur.Bezeichnung,
        StartDatum = inventur.StartDatum,
        AbschlussDatum = inventur.AbschlussDatum,
        Status = inventur.Status,
        Bemerkung = inventur.Bemerkung,
        // Other properties...
        
        Positionen = inventur.Positionen?.Select(p => new InventurPositionDto
        {
            Id = p.Id,
            ArtikelId = p.ArtikelId,
            ArtikelName = p.Artikel?.Name ?? "Unbekannt", // Use the Artikel entity
            ArtikelPreis = p.Artikel?.Preis ?? 0,
            SystemMenge = p.Menge,
            GezaehlteMenge = p.GezaehlteMenge,
            IstGeprueft = p.IstGeprueft,
            Differenz = p.Differenz,
            DifferenzWert = p.DifferenzWert,
            Bemerkung = p.Bemerkung,
            CreatedBy = p.CreatedBy,
            CreatedOn = p.CreatedOn,
            LastModifiedBy = p.LastModifiedBy,
            LastModifiedOn = p.LastModifiedOn
        }).ToList() ?? new List<InventurPositionDto>()
    };
    
    // Calculate summary statistics
    dto.AnzahlArtikel = dto.Positionen?.Count ?? 0;
    dto.AnzahlGeprueft = dto.Positionen?.Count(p => p.IstGeprueft) ?? 0;
    dto.AnzahlDifferenzen = dto.Positionen?.Count(p => p.Differenz != 0) ?? 0;
    dto.GesamtDifferenzWert = dto.Positionen?.Sum(p => p.DifferenzWert ?? 0) ?? 0;
    
    return dto;
}

    public async Task<InventurDto> SchliesseInventurAb(int inventurId)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        
        try
        {
            var inventur = await _dbContext.Inventuren
                .Include(i => i.Berichte)
                .Include(i => i.Positionen)
                .ThenInclude(p => p.Artikel)
                .ThenInclude(a => a.ArtikelStatistik)
                .FirstOrDefaultAsync(i => i.Id == inventurId);
    
            if (inventur == null)
            {
                throw new KeyNotFoundException($"Inventur mit ID {inventurId} nicht gefunden");
            }
    
            if (inventur.Status != InventurStatus.InBearbeitung)
            {
                throw new InvalidOperationException($"Inventur kann nicht abgeschlossen werden, aktueller Status: {inventur.Status}");
            }
    
            // Alle Positionen sollten geprüft sein
            var ungeprueftePositionen = inventur.Positionen.Where(p => !p.IstGeprueft).ToList();
            if (ungeprueftePositionen.Any())
            {
                throw new InvalidOperationException($"Es gibt noch {ungeprueftePositionen.Count} ungeprüfte Positionen");
            }
    
            // Artikel aktualisieren und Inventurhistorie erstellen
            var historieEintraege = new List<ArtikelInventurHistorie>();
            
            foreach (var position in inventur.Positionen)
            {
                if (position.GezaehlteMenge.HasValue && position.Artikel != null)
                {
                    var artikel = position.Artikel;
                    var alteBestandsmenge = artikel.Menge;
                    var neueBestandsmenge = position.GezaehlteMenge.Value;
                    var differenz = neueBestandsmenge - alteBestandsmenge;
                    
                    // Differenzwert berechnen, falls nicht vorhanden
                    if (!position.DifferenzWert.HasValue)
                    {
                        position.DifferenzWert = differenz * artikel.Preis;
                    }
                    
                    // Nur bei Differenz Artikel aktualisieren
                    if (differenz != 0) 
                    {
                        artikel.Menge = neueBestandsmenge;
                        artikel.LastModifiedOn = DateTime.UtcNow;
                        artikel.LastModifiedBy = inventur.LastModifiedBy;
    
                        // Auch die ArtikelStatistik aktualisieren, falls vorhanden
                        if (artikel.ArtikelStatistik != null)
                        {
                            artikel.ArtikelStatistik.Gesamtmenge = neueBestandsmenge;
                            artikel.ArtikelStatistik.Lagerwert = neueBestandsmenge * artikel.ArtikelStatistik.DurchschnittlicherEinzelpreis;
                            artikel.ArtikelStatistik.LastModifiedOn = DateTime.UtcNow;
                            artikel.ArtikelStatistik.LastModifiedBy = inventur.LastModifiedBy;
                        }
                        
                        // Historischen Eintrag erstellen
                        historieEintraege.Add(new ArtikelInventurHistorie
                        {
                            ArtikelId = artikel.Id,
                            InventurId = inventurId,
                            AlteBestandsmenge = alteBestandsmenge,
                            NeueBestandsmenge = neueBestandsmenge,
                            Differenz = differenz,
                            DifferenzWert = position.DifferenzWert.Value,
                            Datum = DateTime.UtcNow
                        });
                    }
                    _dbContext.Entry(artikel).State = EntityState.Modified;
                }
            }
    
            // Historieneinträge speichern, falls vorhanden
            if (historieEintraege.Any())
            {
                await _dbContext.ArtikelInventurHistorie.AddRangeAsync(historieEintraege);
            }
            
            // Inventur abschließen
            inventur.Status = InventurStatus.Abgeschlossen;
            inventur.AbschlussDatum = DateTime.UtcNow;
            inventur.LastModifiedOn = DateTime.UtcNow;
            
            // Inventurbericht erstellen
            await GenerateInventurBericht(inventurId, inventur.LastModifiedBy ?? "System");
            _dbContext.Entry(inventur).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
    
            return MapToInventurDto(inventur);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Abschließen der Inventur");
            await transaction.RollbackAsync();
            throw;
        }
    }

    private InventurBerichtDto MapToInventurBerichtDto(InventurBerichte bericht)
    {
        return new InventurBerichtDto
        {
            Id = bericht.Id,
            InventurId = bericht.InventurId,
            Titel = bericht.Titel,
            Inhalt = bericht.Inhalt,
            Erstellungsdatum = bericht.Erstellungsdatum,
            GesamtDifferenzWert = bericht.GesamtDifferenzWert,
            AnzahlPositionenMitDifferenz = bericht.AnzahlPositionenMitDifferenz,
            InventurBezeichnung = bericht.Inventur?.Bezeichnung,
            InventurStartDatum = bericht.Inventur?.StartDatum ?? DateTime.MinValue,
            InventurAbschlussDatum = bericht.Inventur?.AbschlussDatum,
            CreatedBy = bericht.CreatedBy,
            CreatedOn = bericht.CreatedOn,
            LastModifiedBy = bericht.LastModifiedBy,
            LastModifiedOn = bericht.LastModifiedOn
        };
    }

    private InventurPositionDto MapToInventurPositionDto(InventurPosition position)
    {
        return new InventurPositionDto
        {
            Id = position.Id,
            ArtikelId = position.ArtikelId,
            ArtikelName = position.Artikel?.Name ?? "Unbekannt",
            ArtikelPreis = position.Artikel?.Preis ?? 0,
            SystemMenge = position.Menge,
            GezaehlteMenge = position.GezaehlteMenge,
            IstGeprueft = position.IstGeprueft,
            Differenz = position.Differenz,
            DifferenzWert = position.DifferenzWert,
            Bemerkung = position.Bemerkung,
            CreatedBy = position.CreatedBy,
            CreatedOn = position.CreatedOn,
            LastModifiedBy = position.LastModifiedBy,
            LastModifiedOn = position.LastModifiedOn
        };




    }
}