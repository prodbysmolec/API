using System;
using Artikelsystem.Api.Features.Lieferant.Models.DTOs;
using Artikelsystem.Api.Features.Lieferant.Services;
using Artikelsystem.API.Features.Lieferant.Models.DTOs.Request;
using Artikelsystem.API.Shared.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Artikelsystem.Api.Features.Lieferant.Controllers;

public class LieferantenController : BaseController
{
     private readonly ILieferantService _service;
    private readonly ILogger<LieferantenController> _logger;

    public LieferantenController(
        ILieferantService service,
        ILogger<LieferantenController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<LieferantDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllLieferanten([FromQuery] GetAllLieferantenRequest? request)
    {
        _logger.LogInformation("GetAllLieferanten aufgerufen mit: nurAktive={NurAktive}, alle={Alle}", 
            request?.nurAktive, request?.alle);

        var lieferanten = await _service.GetAllLieferanten(request?.nurAktive ?? false, request?.alle ?? false);

        if (!lieferanten.Any())
        {
            _logger.LogWarning("Keine Lieferanten gefunden");
            return NotFound("Es existieren keine Lieferanten.");
        }
        
        _logger.LogInformation("{Count} Lieferanten gefunden", lieferanten.Count);
        return Ok(lieferanten);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LieferantDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLieferantById(int id, [FromQuery] GetLieferantByIdRequest? request)
    {
        _logger.LogInformation("GetLieferantById aufgerufen für ID {Id} mit alle={Alle}", id, request?.alle);
        
        var lieferant = await _service.GetLieferantById(request?.alle, id);

        if (lieferant == null)
        {
            _logger.LogWarning("Lieferant mit ID {Id} nicht gefunden", id);
            return NotFound($"Lieferant mit ID {id} wurde nicht gefunden.");
        }

        return Ok(lieferant);
    }

    [HttpPost]
    [ProducesResponseType(typeof(LieferantDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateLieferant([FromBody] CreateLieferantRequest request)
    {
        _logger.LogInformation("CreateLieferant aufgerufen für Firma {Firma}", request.Firma);
        
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Ungültige Anfrage bei CreateLieferant: {Errors}", 
                string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            return BadRequest(ModelState);
        }

        var createdLieferant = await _service.ErstelleLieferant(request);

        _logger.LogInformation("Lieferant erstellt mit ID {Id}", createdLieferant.Id);
        return CreatedAtAction(
            nameof(GetLieferantById), 
            new { id = createdLieferant.Id }, 
            createdLieferant
        );
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(LieferantDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateLieferant(int id, [FromBody] UpdateLieferantRequest request)
    {
        _logger.LogInformation("UpdateLieferant aufgerufen für ID {Id}", id);
        
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Ungültige Anfrage bei UpdateLieferant: {Errors}", 
                string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            return BadRequest(ModelState);
        }

        var updatedLieferant = await _service.UpdateLieferantAsync(id, request);

        if (updatedLieferant == null)
        {
            _logger.LogWarning("Lieferant mit ID {Id} wurde beim Update nicht gefunden", id);
            return NotFound($"Lieferant mit ID {id} wurde nicht gefunden.");
        }

        _logger.LogInformation("Lieferant mit ID {Id} wurde aktualisiert", id);
        return Ok(updatedLieferant);
    }

    [HttpPatch("{id}/deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeactivateLieferant(int id)
    {
        _logger.LogInformation("DeactivateLieferant aufgerufen für ID {Id}", id);
        
        var result = await _service.DeactivateLieferantAsync(id);

        if (!result)
        {
            _logger.LogWarning("Lieferant mit ID {Id} wurde bei Deaktivierung nicht gefunden", id);
            return NotFound($"Lieferant mit ID {id} wurde nicht gefunden.");
        }

        _logger.LogInformation("Lieferant mit ID {Id} wurde deaktiviert", id);
        return Ok(new { message = $"Lieferant mit ID {id} wurde deaktiviert." });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteLieferant(int id)
    {
        _logger.LogInformation("DeleteLieferant aufgerufen für ID {Id}", id);
        
        try
        {
            var result = await _service.DeleteLieferantAsync(id);

            if (!result)
            {
                _logger.LogWarning("Lieferant mit ID {Id} wurde beim Löschen nicht gefunden", id);
                return NotFound($"Lieferant mit ID {id} wurde nicht gefunden.");
            }

            _logger.LogInformation("Lieferant mit ID {Id} wurde gelöscht", id);
            return Ok(new { message = $"Lieferant mit ID {id} wurde gelöscht." });
        }
        catch (NullReferenceException ex)
        {
            _logger.LogWarning(ex, "Fehler beim Löschen von Lieferant mit ID {Id}", id);
            return NotFound(ex.Message);
        }
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(List<LieferantDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchLieferanten([FromQuery] string suchbegriff)
    {
        _logger.LogInformation("SearchLieferanten aufgerufen mit Suchbegriff: {Suchbegriff}", 
            string.IsNullOrEmpty(suchbegriff) ? "(leer)" : suchbegriff);
        
        var lieferanten = await _service.SearchLieferantenAsync(suchbegriff);
        
        _logger.LogInformation("{Count} Lieferanten bei Suche nach '{Suchbegriff}' gefunden", 
            lieferanten.Count, suchbegriff);
            
        return Ok(lieferanten);
    }
}