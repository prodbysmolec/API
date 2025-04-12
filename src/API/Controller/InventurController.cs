// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Artikelsystem.Shared.DTOs.Inventur;
// using API.Features.Inventur.Services;
// using API.Common.Controllers;
// using FluentValidation;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;

// // namespace API.Controller;

// public class InventurController : BaseController
// {
//     private readonly IInventurService _inventurService;
//     private readonly ILogger<InventurController> _logger;

//     public InventurController(
//         IInventurService inventurService,
//         ILogger<InventurController> logger)
//     {
//         _inventurService = inventurService;
//         _logger = logger;
//     }

//     /// <summary>
//     /// Holt alle Inventuren.
//     /// </summary>
//     /// <returns></returns>
//     [HttpGet]
//     [ProducesResponseType(typeof(List<InventurDto>), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> GetAlleInventuren()
//     {
//         var inventuren = await _inventurService.GetAlleInventuren();
//         return Ok(inventuren);
//     }

//     /// <summary>
//     /// Holt die Inventur basierend auf der ID
//     /// </summary>
//     /// <param name="id"></param>
//     /// <returns></returns>
//     [HttpGet("{id:int}")]
//     [ProducesResponseType(typeof(InventurDto), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> GetInventurById(int id)
//     {
//         try
//         {
//             var inventur = await _inventurService.GetInventurById(id);
//             return Ok(inventur);
//         }
//         catch (KeyNotFoundException)
//         {
//             return NotFound();
//         }
//         catch (ValidationException ex)
//         {
//             return BadRequest(ex.Errors);
//         }
//     }

//     /// <summary>
//     /// Erstellt eine Inventur
//     /// </summary>
//     /// <param name="request"></param>
//     /// <returns></returns>
//     [HttpPost]
//     [ProducesResponseType(typeof(InventurDto), StatusCodes.Status201Created)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> ErstelleInventur(CreateInventurRequest request)
//     {
//         try
//         {
//             var inventur = await _inventurService.ErstelleInventur(request);
//             return CreatedAtAction(nameof(GetInventurById), new { id = inventur.Id }, inventur);
//         }
//         catch (ValidationException ex)
//         {
//             return BadRequest(ex.Errors);
//         }
//     }

//     /// <summary>
//     /// Startet eine Inventur basierend auf der ID.
//     /// </summary>
//     /// <param name="id"></param>
//     /// <returns></returns>
//     [HttpPost("{id:int}/starten")]
//     [ProducesResponseType(typeof(InventurDto), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> StarteInventur(int id)
//     {
//         try
//         {
//             var inventur = await _inventurService.StarteInventur(id);
//             return Ok(inventur);
//         }
//         catch (KeyNotFoundException)
//         {
//             return NotFound();
//         }
//         catch (ValidationException ex)
//         {
//             return BadRequest(ex.Errors);
//         }
//         catch (InvalidOperationException ex)
//         {
//             return BadRequest(ex.Message);
//         }
//     }

//     [HttpPut("positionen")]
//     [ProducesResponseType(typeof(InventurPositionDto), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> AktualisieereInventurPosition(UpdateInventurPositionRequest request)
//     {
//         try
//         {
//             var position = await _inventurService.AktualisieereInventurPosition(request);
//             return Ok(position);
//         }
//         catch (KeyNotFoundException)
//         {
//             return NotFound();
//         }
//         catch (ValidationException ex)
//         {
//             return BadRequest(ex.Errors);
//         }
//         catch (InvalidOperationException ex)
//         {
//             return BadRequest(ex.Message);
//         }
//     }

//     // Ergänzungen für den InventurController

//     [HttpGet("berichte")]
//     [ProducesResponseType(typeof(List<InventurBerichtDto>), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> GetInventurBerichte()
//     {
//         try
//         {
//             var berichte = await _inventurService.GetInventurBerichte();
//             return Ok(berichte);
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "Fehler beim Abrufen der Inventurberichte");
//             return StatusCode(500, "Ein Fehler ist aufgetreten");
//         }
//     }

//     [HttpGet("{id:int}/bericht")]
//     [ProducesResponseType(typeof(InventurBerichtDto), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> GetInventurBericht(int id)
//     {
//         try
//         {
//             var bericht = await _inventurService.GetInventurBerichtFuerInventur(id);
//             if (bericht == null)
//                 return NotFound();

//             return Ok(bericht);
//         }
//         catch (ValidationException ex)
//         {
//             return BadRequest(ex.Errors);
//         }
//         catch (KeyNotFoundException)
//         {
//             return NotFound();
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "Fehler beim Abrufen des Inventurberichts");
//             return StatusCode(500, "Ein Fehler ist aufgetreten");
//         }
//     }

//     [HttpGet("berichte/{id:int}")]
//     [ProducesResponseType(typeof(InventurBerichtDto), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> GetBerichtById(int id)
//     {
//         try
//         {
//             var bericht = await _inventurService.GetInventurBerichtById(id);
//             return Ok(bericht);
//         }
//         catch (ValidationException ex)
//         {
//             return BadRequest(ex.Errors);
//         }
//         catch (KeyNotFoundException)
//         {
//             return NotFound();
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "Fehler beim Abrufen des Inventurberichts");
//             return StatusCode(500, "Ein Fehler ist aufgetreten");
//         }
//     }

//     [HttpPost("{id:int}/abschliessen")]
//     [ProducesResponseType(typeof(InventurDto), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> SchliesseInventurAb(int id)
//     {
//         try
//         {
//             var inventur = await _inventurService.SchliesseInventurAb(id);
//             return Ok(inventur);
//         }
//         catch (ValidationException ex)
//         {
//             return BadRequest(ex.Errors);
//         }
//         catch (KeyNotFoundException)
//         {
//             return NotFound();
//         }
//         catch (InvalidOperationException ex)
//         {
//             return BadRequest(ex.Message);
//         }
//     }

//     [HttpDelete("{id}")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> EntferneInventur(int id)
//     {
//         var Inventur = await _inventurService.DeleteInventur(id);
//         if (Inventur == null)
//         {
//             return NotFound("Inventur konnte nicht gelöscht werden. Es existiert keine Inventur mit der angegebenen ID.");
//         }

//         return Ok("Inventur wurde gelöscht.");
//     }

// }