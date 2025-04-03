using System.Collections.Generic;
using System.Threading.Tasks;
using Artikelsystem.Api.Features.Inventur.Models.Dtos;
using Artikelsystem.Api.Features.Inventur.Services;
using Artikelsystem.API.Shared.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Artikelsystem.Api.Features.Inventur.Controllers;

public class InventurController : BaseController
{
    private readonly IInventurService _inventurService;
    private readonly ILogger<InventurController> _logger;

    public InventurController(
        IInventurService inventurService,
        ILogger<InventurController> logger)
    {
        _inventurService = inventurService;
        _logger = logger;
    }

    /// <summary>
    /// Holt alle Inventuren.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<InventurDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAlleInventuren()
    {
        var inventuren = await _inventurService.GetAlleInventuren();
        return Ok(inventuren);
    }

    /// <summary>
    /// Holt die Inventur basierend auf der ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(InventurDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetInventurById(int id)
    {
        try
        {
            var inventur = await _inventurService.GetInventurById(id);
            return Ok(inventur);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Erstellt eine Inventur
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(InventurDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ErstelleInventur(CreateInventurRequest request)
    {
        var inventur = await _inventurService.ErstelleInventur(request);
        return CreatedAtAction(nameof(GetInventurById), new { id = inventur.Id }, inventur);
    }

    /// <summary>
    /// Startet eine Inventur basierend auf der ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost("{id:int}/starten")]
    [ProducesResponseType(typeof(InventurDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> StarteInventur(int id)
    {
        try
        {
            var inventur = await _inventurService.StarteInventur(id);
            return Ok(inventur);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("positionen")]
    [ProducesResponseType(typeof(InventurPositionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AktualisieereInventurPosition(UpdateInventurPositionRequest request)
    {
        try
        {
            var position = await _inventurService.AktualisieereInventurPosition(request);
            return Ok(position);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Ergänzungen für den InventurController

    [HttpGet("berichte")]
    [ProducesResponseType(typeof(List<InventurBerichtDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetInventurBerichte()
    {
        try
        {
            var berichte = await _inventurService.GetInventurBerichte();
            return Ok(berichte);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Abrufen der Inventurberichte");
            return StatusCode(500, "Ein Fehler ist aufgetreten");
        }
    }

    [HttpGet("{id:int}/bericht")]
    [ProducesResponseType(typeof(InventurBerichtDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetInventurBericht(int id)
    {
        try
        {
            var bericht = await _inventurService.GetInventurBerichtFuerInventur(id);
            if (bericht == null)
                return NotFound();
                
            return Ok(bericht);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Abrufen des Inventurberichts");
            return StatusCode(500, "Ein Fehler ist aufgetreten");
        }
    }

    [HttpGet("berichte/{id:int}")]
    [ProducesResponseType(typeof(InventurBerichtDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBerichtById(int id)
    {
        try
        {
            var bericht = await _inventurService.GetInventurBerichtById(id);
            return Ok(bericht);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Abrufen des Inventurberichts");
            return StatusCode(500, "Ein Fehler ist aufgetreten");
        }
    }

    [HttpPost("{id:int}/abschliessen")]
    [ProducesResponseType(typeof(InventurDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SchliesseInventurAb(int id)
    {
        try
        {
            var inventur = await _inventurService.SchliesseInventurAb(id);
            return Ok(inventur);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}