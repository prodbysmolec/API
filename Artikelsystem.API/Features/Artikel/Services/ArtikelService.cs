using Artikelsystem.Api.Features.Artikel.Models.DTOs;
using Artikelsystem.Api.Features.Artikel.Models.Entitys;
using Artikelsystem.Api.Features.Employees.Enums;
using Artikelsystem.Api.Infrastructure.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Http;

namespace Artikelsystem.Api.Features.Artikel.Services;

public class ArtikelService : IArtikelService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ArtikelService> _logger;

    public ArtikelService(IUnitOfWork unitOfWork, ILogger<ArtikelService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<ArtikelDto>> GetAllArtikelAsync()
    {
        var artikel = await _unitOfWork.ArtikelRepository.GetAllAsync();
        return artikel.Select(MapToDto);
    }

    public async Task<ArtikelDto?> GetArtikelByIdAsync(int id)
    {
        var artikel = await _unitOfWork.ArtikelRepository.GetArtikelMitStatistikAsync(id);
        return artikel != null ? MapToDto(artikel) : null;
    }

    public async Task<ArtikelDto> CreateArtikelAsync(CreateArtikelRequest request)
    {
        var artikel = new Models.Entitys.Artikel
        {
            Name = request.Name,
            Preis = request.Preis,
            Mindestbestand = request.Mindestbestand,
            Maximalbestand = request.Maximalbestand,
            Menge = request.Menge,
            Status = request.Status,
            Bild = request.BildFile != null ? await ConvertFormFileToByteArray(request.BildFile) : new byte[0]
        };

        await _unitOfWork.ArtikelRepository.AddAsync(artikel);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(artikel);
    }

    public async Task<ArtikelDto?> UpdateArtikelAsync(int id, UpdateArtikelRequest request)
    {
        var artikel = await _unitOfWork.ArtikelRepository.GetByIdAsync(id);
        if (artikel == null)
        {
            return null;
        }

        // Nur Felder aktualisieren, die im Request enthalten sind
        if (request.Name != null) artikel.Name = request.Name;
        if (request.Preis.HasValue) artikel.Preis = request.Preis.Value;
        if (request.Mindestbestand.HasValue) artikel.Mindestbestand = request.Mindestbestand.Value;
        if (request.Maximalbestand.HasValue) artikel.Maximalbestand = request.Maximalbestand.Value;
        if (request.Menge.HasValue) artikel.Menge = request.Menge.Value;
        if (request.Status.HasValue) artikel.Status = request.Status.Value;

        // Bild aktualisieren
        if (request.BildFile != null)
        {
            artikel.Bild = await ConvertFormFileToByteArray(request.BildFile);
        }
        else if (request.EntferneBild)
        {
            artikel.Bild = new byte[0];
        }

        _unitOfWork.ArtikelRepository.Update(artikel);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(artikel);
    }

    public async Task<bool> DeleteArtikelAsync(int id)
    {
        var artikel = await _unitOfWork.ArtikelRepository.GetByIdAsync(id);
        if (artikel == null)
        {
            return false;
        }
    
        // Prüfen, ob der Artikel in Wareneingängen verwendet wird
        var wareneingaenge = await _unitOfWork.WareneingangArtikelRepository.GetByArtikelIdAsync(id);
        if (wareneingaenge.Any())
        {
            _logger.LogWarning("Artikel mit ID {ArtikelId} kann nicht gelöscht werden, da er in Wareneingängen verwendet wird", id);
            return false;
        }

        _unitOfWork.ArtikelRepository.Delete(artikel);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<ArtikelDto>> GetArtikelMitBestandUnterMindestbestandAsync()
    {
        var artikel = await _unitOfWork.ArtikelRepository.GetArtikelMitBestandUnterMindestbestandAsync();
        return artikel.Select(MapToDto);
    }

    public async Task<bool> UpdateArtikelBestandAsync(int artikelId, int menge)
    {
        var result = await _unitOfWork.ArtikelRepository.UpdateArtikelBestandAsync(artikelId, menge);
        if (result)
        {
            await _unitOfWork.SaveChangesAsync();
        }
        return result;
    }

    public async Task<bool> UpdateArtikelStatusAsync(int artikelId, ArtikelStatus status)
    {
        var artikel = await _unitOfWork.ArtikelRepository.GetByIdAsync(artikelId);
        if (artikel == null)
        {
            return false;
        }

        artikel.Status = status;
        _unitOfWork.ArtikelRepository.Update(artikel);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AktualisiereArtikelStatistikAsync(int artikelId)
    {
        var result = await _unitOfWork.ArtikelStatistikRepository.AktualisiereStatistikNachWareneingangAsync(artikelId);
        if (result)
        {
            await _unitOfWork.SaveChangesAsync();
        }
        return result;
    }

    private async Task<byte[]> ConvertFormFileToByteArray(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    private ArtikelDto MapToDto(Models.Entitys.Artikel artikel)
    {
        return new ArtikelDto
        {
            Id = artikel.Id,
            Name = artikel.Name,
            Preis = artikel.Preis,
            Mindestbestand = artikel.Mindestbestand,
            Maximalbestand = artikel.Maximalbestand,
            Menge = artikel.Menge,
            Status = artikel.Status,
            Bild = artikel.Bild,
            ArtikelStatistik = artikel.ArtikelStatistik != null ? new ArtikelStatistikDto
            {
                Id = artikel.ArtikelStatistik.Id,
                ArtikelId = artikel.ArtikelStatistik.ArtikelId,
                Gesamtmenge = artikel.ArtikelStatistik.Gesamtmenge,
                DurchschnittlicherEinzelpreis = artikel.ArtikelStatistik.DurchschnittlicherEinzelpreis,
                DurchschnittlicherVerkaufspreis = artikel.ArtikelStatistik.DurchschnittlicherVerkaufspreis,
                VerkaufsMenge = artikel.ArtikelStatistik.VerkaufsMenge,
                Lagerwert = artikel.ArtikelStatistik.Lagerwert,
                GesamtVerkaufswert = artikel.ArtikelStatistik.GesamtVerkaufswert
            } : null
        };
    }
}