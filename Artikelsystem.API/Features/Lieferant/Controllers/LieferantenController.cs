using System;
using Artikelsystem.Api.Features.Lieferant.Models.DTOs;
using Artikelsystem.Api.Features.Lieferant.Services;
using Microsoft.AspNetCore.Mvc;

namespace Artikelsystem.Api.Features.Lieferant.Controllers;

public class LieferantController : ControllerBase
{
    private readonly LieferantService _service;

    public LieferantController(LieferantService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<LieferantDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllLieferanten([FromQuery] bool nurAktive = false, bool alle = false)
    {
        var lieferanten = await _service.GetAllLieferanten(nurAktive, alle);
        if (!lieferanten.Any())
        {
            return NotFound("Es existieren keine Lieferanten.");
        }
        return Ok(lieferanten);
    }

    // [HttpGet("{id}")]
    // public async Task<IActionResult> GetLieferantById(int id)
    // {
    //     var lieferant = await _service.GetLieferantByIdAsync(id);

    //     if (lieferant == null)
    //     {
    //         return NotFound($"Lieferant mit ID {id} wurde nicht gefunden.");
    //     }

    //     return Ok(MapToDetailDto(lieferant));
    // }

    // [HttpPost]
    // public async Task<IActionResult> CreateLieferant([FromBody] CreateLieferantRequest request)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return BadRequest(ModelState);
    //     }

    //     var lieferant = new Models.Entitys.Lieferant
    //     {
    //         Firma = request.Firma,
    //         Name = request.Name,
    //         Vorname = request.Vorname,
    //         EmailAdresse = request.EmailAdresse,
    //         Strasse = request.Strasse,
    //         Hausnummer = request.Hausnummer,
    //         PLZ = request.PLZ,
    //         Ort = request.Ort,
    //         Telefonnummer = request.Telefonnummer,
    //         Notizen = request.Notizen,
    //         IstAktiv = true
    //     };

    //     var createdLieferant = await _service.CreateLieferantAsync(lieferant);

    //     return CreatedAtAction(
    //         nameof(GetLieferantById), 
    //         new { id = createdLieferant.Id }, 
    //         MapToDto(createdLieferant)
    //     );
    // }

    // [HttpPut("{id}")]
    // public async Task<IActionResult> UpdateLieferant(int id, [FromBody] UpdateLieferantRequest request)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return BadRequest(ModelState);
    //     }

    //     var lieferant = new Models.Entitys.Lieferant
    //     {
    //         Firma = request.Firma,
    //         Name = request.Name,
    //         Vorname = request.Vorname,
    //         EmailAdresse = request.EmailAdresse,
    //         Strasse = request.Strasse,
    //         Hausnummer = request.Hausnummer,
    //         PLZ = request.PLZ,
    //         Ort = request.Ort,
    //         Telefonnummer = request.Telefonnummer,
    //         Notizen = request.Notizen,
    //         IstAktiv = request.IstAktiv
    //     };

    //     var updatedLieferant = await _service.UpdateLieferantAsync(id, MapToDto(lieferant));

    //     if (updatedLieferant == null)
    //     {
    //         return NotFound($"Lieferant mit ID {id} wurde nicht gefunden.");
    //     }

    //     return Ok(MapToDto(updatedLieferant));
    // }

    // [HttpPatch("{id}/deactivate")]
    // public async Task<IActionResult> DeactivateLieferant(int id)
    // {
    //     var result = await _service.DeactivateLieferantAsync(id);

    //     if (!result)
    //     {
    //         return NotFound($"Lieferant mit ID {id} wurde nicht gefunden.");
    //     }

    //     return Ok(new { message = $"Lieferant mit ID {id} wurde deaktiviert." });
    // }

    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeleteLieferant(int id)
    // {
    //     var result = await _service.DeleteLieferantAsync(id);

    //     if (!result)
    //     {
    //         return NotFound($"Lieferant mit ID {id} wurde nicht gefunden.");
    //     }

    //     return Ok(new { message = $"Lieferant mit ID {id} wurde gelöscht." });
    // }

    // [HttpGet("search")]
    // public async Task<IActionResult> SearchLieferanten([FromQuery] string suchbegriff)
    // {
    //     var lieferanten = await _service.SearchLieferantenAsync(suchbegriff);
    //     var result = lieferanten.Select(l => MapToDto(l)).ToList();
    //     return Ok(result);
    // }

    #region Helper Methods

    private LieferantDto MapToDto(Models.Entitys.Lieferant lieferant)
    {
        return new LieferantDto
        {
            Id = lieferant.Id,
            Firma = lieferant.Firma,
            Name = lieferant.Name,
            Vorname = lieferant.Vorname,
            EmailAdresse = lieferant.EmailAdresse,
            Strasse = lieferant.Strasse,
            Hausnummer = lieferant.Hausnummer,
            PLZ = lieferant.PLZ,
            Ort = lieferant.Ort,
            Telefonnummer = lieferant.Telefonnummer,
            Notizen = lieferant.Notizen,
            IstAktiv = lieferant.IstAktiv,
            AdresseFormatiert = $"{lieferant.Strasse} {lieferant.Hausnummer}, {lieferant.PLZ} {lieferant.Ort}"
        };
    }

    private LieferantDetailDto MapToDetailDto(Models.Entitys.Lieferant lieferant)
    {
        return new LieferantDetailDto
        {
            Id = lieferant.Id,
            Firma = lieferant.Firma,
            Name = lieferant.Name,
            Vorname = lieferant.Vorname,
            EmailAdresse = lieferant.EmailAdresse,
            Strasse = lieferant.Strasse,
            Hausnummer = lieferant.Hausnummer,
            PLZ = lieferant.PLZ,
            Ort = lieferant.Ort,
            Telefonnummer = lieferant.Telefonnummer,
            Notizen = lieferant.Notizen,
            IstAktiv = lieferant.IstAktiv,
            AdresseFormatiert = $"{lieferant.Strasse} {lieferant.Hausnummer}, {lieferant.PLZ} {lieferant.Ort}",
            ArtikelAnzahl = lieferant.ArtikelLieferanten?.Count(al => al.IstAktiv) ?? 0,
            AktiveArtikel = lieferant.ArtikelLieferanten?
                .Where(al => al.IstAktiv)
                .Select(al => new LieferantArtikelDto
                {
                    ArtikelId = al.ArtikelId,
                    ArtikelName = al.Artikel?.Name ?? "Unbekannter Artikel",
                    Einkaufspreis = al.Einkaufspreis,
                    IstPrimaerLieferant = al.IstPrimaerLieferant,
                    ArtikelNrBeimLieferanten = al.ArtikelNrBeimLieferanten,
                    SeitDatum = al.GueltigVon
                })
                .ToList() ?? new List<LieferantArtikelDto>()
        };
        #endregion
    }
}