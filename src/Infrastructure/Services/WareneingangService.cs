using System;
using Application.Interfaces.Services;
using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Request;
using Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Response;
using AutoMapper;
using Domain.Entities.Wareneingang;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class WareneingangService(AppDbContext context, IMapper mapper) : IWareneingangService
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<int> AddWareneingangsPositionAsync(AddWareneingangsPositionRequest request)
    {
        var wareneingang = await _context.Wareneingaenge
            .Include(w => w.WareneingangsPositionen)
            .FirstOrDefaultAsync(w => w.Id == request.WareneingangId);

        if (wareneingang == null)
        {
            throw new ArgumentException($"Wareneingang mit ID {request.WareneingangId} nicht gefunden.");
        }

        var artikel = await _context.Artikel.FirstOrDefaultAsync(a => a.Id == request.ArtikelId);

        if (artikel == null)
        {
            throw new ArgumentException($"Artikel mit ID {request.ArtikelId} nicht gefunden.");
        }

        var neuePosition = new WareneingangArtikelPositionen
        {
            ArtikelId = request.ArtikelId,
            WareneingangId = request.WareneingangId,
            Menge = request.Menge,
            Einzelpreis = request.Einzelpreis,
            Gesamtpreis = request.Menge * request.Einzelpreis
        };

        wareneingang.WareneingangsPositionen.Add(neuePosition);
        wareneingang.Gesamtpreis += neuePosition.Gesamtpreis;

        _context.Wareneingaenge.Update(wareneingang);
        return await _context.SaveChangesAsync();
    }

    public async Task<PagedResultDTO<GetAlleWareneingaengeResponse>> GetAlleWareneingaengeAsync(int page, int recordsPerPage)
    {
        IQueryable<Domain.Entities.Wareneingang.Wareneingaenge> wareneingaenge = _context.Wareneingaenge
            .Include(w => w.WareneingangsPositionen)
            .ThenInclude(p => p.Artikel);

        // Validierung der Paging-Parameter
        if (page <= 0 || recordsPerPage <= 0)
        {
            throw new ArgumentException("Page and RecordsPerPage must be greater than zero.");
        }

        // Paging anwenden
        var totalRecords = await wareneingaenge.CountAsync();
        var totalPages = (int)Math.Ceiling(totalRecords / (double)recordsPerPage);

        var pagedData = await wareneingaenge
            .Skip((page - 1) * recordsPerPage)
            .Take(recordsPerPage)
            .ToListAsync();

        // Mapping der Ergebnisse in DTOs
        var wareneingaengeDtos = pagedData.Select(we => new GetAlleWareneingaengeResponse
        {
            WareneingangId = we.Id,
            Datum = we.ErstelltAm, // Beispiel: Datum des Wareneingangs
            Gesamtpreis = we.Gesamtpreis,
            AllgemeineBemerkungen = we.AllgemeineBemerkungen,
            ArtikelPositionen = we.WareneingangsPositionen.Select(p => new WareneingangArtikelPositionenDto
            {
                ArtikelId = p.ArtikelId,
                ArtikelName = p.Artikel!.Name,
                Menge = p.Menge,
                Einzelpreis = p.Einzelpreis
            }).ToList()
        }).ToList();

        // Rückgabe eines `PagedResultDTO`
        return new PagedResultDTO<GetAlleWareneingaengeResponse>
        {
            Page = page,
            RecordsPerPage = recordsPerPage,
            TotalRecords = totalRecords,
            Items = wareneingaengeDtos
        };
    }

    public async Task<List<GetWareneingaengeForArtikelResponse>> GetWareneingaengeForArtikelAsync(int artikelId)
    {
        var wareneingaenge = await _context.Wareneingaenge
            .Include(w => w.WareneingangsPositionen)
            .ThenInclude(p => p.Artikel)
            .Where(w => w.WareneingangsPositionen.Any(p => p.ArtikelId == artikelId))
            .ToListAsync();

        return _mapper.Map<List<GetWareneingaengeForArtikelResponse>>(wareneingaenge);
    }
}
