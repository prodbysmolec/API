using Artikelsystem.Domain.Entities.Artikel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artikelsystem.Api.Features.Lieferant.Models.Entitys;
using Artikelsystem.Api.Infrastructure.Persistence.Context;
using Artikelsystem.Api.Features.Lieferant.Models.DTOs;
using Artikelsystem.Api.Features.Lieferant.Controllers;
using Artikelsystem.API.Features.Lieferant.Services;

namespace Artikelsystem.Api.Features.Lieferant.Services;

public class ArtikelLieferantService : IArtikelLieferantService
{
    private readonly AppDbContext _context;

    public ArtikelLieferantService(AppDbContext context)
    {
        _context = context;
    }

        /// <summary>
    /// Holt alle Lieferanten für einen Artikel (aktuell und historisch)
    /// </summary>
    public async Task<List<ArtikelLieferantDto>> GetAllLieferantenForArtikelAsync(int artikelId, bool nurAktive = false)
    {
        var query = _context.ArtikelLieferanten
            .Include(al => al.Lieferant)
            .Where(al => al.ArtikelId == artikelId);

        if (nurAktive)
        {
            query = query.Where(al => al.IstAktiv);
        }

        var beziehungen = await query
            .OrderByDescending(al => al.GueltigVon)
            .ToListAsync();

        return beziehungen.Select(al => MapToDto(al, al.Lieferant)).ToList();
    }

        /// <summary>
    /// Holt den primären/hauptsächlichen Lieferanten für einen Artikel
    /// </summary>
    public async Task<ArtikelLieferantDto?> GetPrimaryLieferantForArtikelAsync(int artikelId)
    {
        var primaerBeziehung = await _context.ArtikelLieferanten
            .Include(al => al.Lieferant)
            .Where(al => al.ArtikelId == artikelId && al.IstAktiv && al.IstPrimaerLieferant)
            .FirstOrDefaultAsync();

        if (primaerBeziehung == null || primaerBeziehung.Lieferant == null)
        {
            return null;
        }

        return MapToDto(primaerBeziehung, primaerBeziehung.Lieferant);
    }


    /// <summary>
    /// Fügt einen neuen Lieferanten für einen Artikel hinzu
    /// </summary>
    public async Task<ArtikelLieferantDto> AddLieferantToArtikelAsync(
        int artikelId, 
        int lieferantId, 
        ArtikelLieferantAddDto dto)
    {
        var artikel = await _context.Artikel.FindAsync(artikelId) 
            ?? throw new KeyNotFoundException($"Artikel mit ID {artikelId} nicht gefunden");

        var lieferant = await _context.Lieferanten.FindAsync(lieferantId)
            ?? throw new KeyNotFoundException($"Lieferant mit ID {lieferantId} nicht gefunden");

        // Wenn dieser als Primärlieferant festgelegt wird, setze alle anderen als nicht primär
        if (dto.IstPrimaer)
        {
            var existingPrimaerLieferanten = await _context.ArtikelLieferanten
                .Where(al => al.ArtikelId == artikelId && al.IstPrimaerLieferant && al.IstAktiv)
                .ToListAsync();

            foreach (var existingPrimaer in existingPrimaerLieferanten)
            {
                existingPrimaer.IstPrimaerLieferant = false;
            }
        }

        

        var artikelLieferant = new ArtikelLieferant
        {
            ArtikelId = artikelId,
            LieferantId = lieferantId,
            Einkaufspreis = dto.Einkaufspreis,
            IstAktiv = true,
            IstPrimaerLieferant = dto.IstPrimaer,
            GueltigVon = DateTime.UtcNow,
            Mindestbestellmenge = dto.Mindestbestellmenge,
            Lieferzeit = dto.Lieferzeit,
            ArtikelNrBeimLieferanten = dto.ArtikelNrBeimLieferanten
        };

        _context.ArtikelLieferanten.Add(artikelLieferant);
        await _context.SaveChangesAsync();

        return MapToDto(artikelLieferant, lieferant);
    }

    /// <summary>
    /// Wechselt den Lieferanten für einen Artikel
    /// </summary>
    public async Task<ArtikelLieferantDto> ChangeLieferantAsync(
        int artikelId, 
        int neuerLieferantId, 
        ArtikelLieferantAddDto dto)
        {
            var artikel = await _context.Artikel.FindAsync(artikelId)
                ?? throw new KeyNotFoundException($"Artikel mit ID {artikelId} nicht gefunden");
    
            var neuerLieferant = await _context.Lieferanten.FindAsync(neuerLieferantId)
                ?? throw new KeyNotFoundException($"Lieferant mit ID {neuerLieferantId} nicht gefunden");

            // Bestehende aktive Lieferantenbeziehungen deaktivieren
            var existingAktiveLieferanten = await _context.ArtikelLieferanten
                .Where(al => al.ArtikelId == artikelId && al.IstAktiv)
                .ToListAsync();

            foreach (var existingLieferant in existingAktiveLieferanten)
            {
                // Markiere als historisch
                existingLieferant.IstAktiv = false;
                existingLieferant.IstPrimaerLieferant = false; // Nicht mehr primär
                existingLieferant.GueltigBis = DateTime.UtcNow;
                _context.Entry(existingLieferant).State = EntityState.Modified;
            }

            // Neuen Lieferanten hinzufügen
            var neuerArtikelLieferant = await AddLieferantToArtikelAsync(
                artikelId, 
                neuerLieferantId, 
                dto);

            _context.Entry(neuerArtikelLieferant).State = EntityState.Modified;

            return neuerArtikelLieferant;
        }


    /// <summary>
    /// Aktualisiert die Lieferantenbeziehung zu einem Artikel
    /// </summary>
    public async Task<ArtikelLieferantDto?> UpdateArtikelLieferantAsync(
        int artikelId,
        int lieferantId,
        ArtikelLieferantUpdateDto dto)
    {
        var beziehung = await _context.ArtikelLieferanten
            .Include(al => al.Lieferant)
            .FirstOrDefaultAsync(al => 
                al.ArtikelId == artikelId && 
                al.LieferantId == lieferantId && 
                al.IstAktiv);

        if (beziehung == null)
        {
            return null;
        }

        // Wenn Primärstatus geändert werden soll und dies der neue Primär sein wird
        if (dto.IstPrimaer && !beziehung.IstPrimaerLieferant)
        {
            // Alle anderen Primärlieferanten deaktivieren
            var anderePrimaere = await _context.ArtikelLieferanten
                .Where(al => 
                    al.ArtikelId == artikelId && 
                    al.IstPrimaerLieferant && 
                    al.IstAktiv &&
                    al.Id != beziehung.Id)
                .ToListAsync();

            foreach (var anderer in anderePrimaere)
            {
                anderer.IstPrimaerLieferant = false;
            }
        }

        // Daten aktualisieren
        beziehung.Einkaufspreis = dto.Einkaufspreis;
        beziehung.Mindestbestellmenge = dto.Mindestbestellmenge;
        beziehung.Lieferzeit = dto.Lieferzeit;
        beziehung.ArtikelNrBeimLieferanten = dto.ArtikelNrBeimLieferanten;
        beziehung.IstPrimaerLieferant = dto.IstPrimaer;

        _context.Entry(beziehung).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return MapToDto(beziehung, beziehung.Lieferant);
    }


    /// <summary>
    /// Deaktiviert eine Lieferantenbeziehung zu einem Artikel
    /// </summary>
    public async Task<bool> DeactivateArtikelLieferantAsync(int artikelId, int lieferantId)
    {
        var beziehung = await _context.ArtikelLieferanten
            .FirstOrDefaultAsync(al => 
                al.ArtikelId == artikelId && 
                al.LieferantId == lieferantId && 
                al.IstAktiv);

        if (beziehung == null)
        {
            return false;
        }

        beziehung.IstAktiv = false;
        beziehung.GueltigBis = DateTime.UtcNow;

        // Wenn dies der primäre Lieferant war, dann gibt es keinen primären Lieferanten mehr
        if (beziehung.IstPrimaerLieferant)
        {
            beziehung.IstPrimaerLieferant = false;
        }

        _context.Entry(beziehung).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return true;
    }


    /// <summary>
    /// Löscht eine Lieferantenbeziehung zu einem Artikel (Hard Delete)
    /// </summary>
    public async Task<bool> DeleteArtikelLieferantAsync(int artikelId, int lieferantId)
    {
        var beziehung = await _context.ArtikelLieferanten
            .FirstOrDefaultAsync(al => 
                al.ArtikelId == artikelId && 
                al.LieferantId == lieferantId);

        if (beziehung == null)
        {
            return false;
        }

        _context.ArtikelLieferanten.Remove(beziehung);
        await _context.SaveChangesAsync();

        return true;
    }


    /// <summary>
    /// Sucht nach Lieferanten für einen bestimmten Artikel anhand von Suchkriterien
    /// </summary>
    public async Task<List<ArtikelLieferantDto>> SearchLieferantenForArtikelAsync(int artikelId, string suchbegriff)
    {
        if (string.IsNullOrEmpty(suchbegriff))
        {
            return await GetAllLieferantenForArtikelAsync(artikelId, true);
        }

        suchbegriff = suchbegriff.ToLower();

        var beziehungen = await _context.ArtikelLieferanten
            .Include(al => al.Lieferant)
            .Where(al => 
                al.ArtikelId == artikelId && 
                (al.Lieferant!.Firma.ToLower().Contains(suchbegriff) ||
                 al.Lieferant.Name.ToLower().Contains(suchbegriff) ||
                 al.Lieferant.Vorname.ToLower().Contains(suchbegriff) ||
                 al.ArtikelNrBeimLieferanten!.ToLower().Contains(suchbegriff)))
            .OrderByDescending(al => al.IstAktiv)
            .ThenByDescending(al => al.GueltigVon)
            .ToListAsync();

        return beziehungen.Select(al => MapToDto(al, al.Lieferant)).ToList();
    }

    /// <summary>
    /// Holt alle Artikelbeziehungen für einen bestimmten Lieferanten
    /// </summary>
    public async Task<List<ArtikelLieferantDto>> GetArtikelByLieferantAsync(int lieferantId, bool nurAktive = true)
    {
        var query = _context.ArtikelLieferanten
            .Include(al => al.Artikel)
            .Where(al => al.LieferantId == lieferantId);

        if (nurAktive)
        {
            query = query.Where(al => al.IstAktiv);
        }

        var beziehungen = await query
            .OrderBy(al => al.Artikel!.Name)
            .ToListAsync();

        return beziehungen.Select(al => 
        {
            var dto = MapToDto(al, null);
            dto.ArtikelName = al.Artikel?.Name ?? "Unbekannter Artikel";
            return dto;
        }).ToList();
    }

    private ArtikelLieferantDto MapToDto(ArtikelLieferant al, Lieferant.Models.Entitys.Lieferant? lieferant)
    {
        return new ArtikelLieferantDto
        {
            Id = al.Id,
            ArtikelId = al.ArtikelId,
            LieferantId = al.LieferantId,
            LieferantFirma = lieferant?.Firma ?? string.Empty,
            LieferantName = lieferant?.Name ?? string.Empty,
            LieferantVorname = lieferant?.Vorname ?? string.Empty,
            Einkaufspreis = al.Einkaufspreis,
            Mindestbestellmenge = al.Mindestbestellmenge,
            Lieferzeit = al.Lieferzeit,
            ArtikelNrBeimLieferanten = al.ArtikelNrBeimLieferanten,
            IstAktiv = al.IstAktiv,
            IstPrimaerLieferant = al.IstPrimaerLieferant,
            GueltigVon = al.GueltigVon,
            GueltigBis = al.GueltigBis
        };
    }
}

