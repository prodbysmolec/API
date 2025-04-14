using API.Common.Controllers;
using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Filter;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Queries.Warenausgaenge;
using Domain.Common.ResultPattern;


namespace API.Features.Warenausgang.Controllers;
public class WarenausgangController(IMediator mediator, ILogger<WarenausgangController> logger) : BaseController
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<WarenausgangController> _logger = logger;

    
    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDTO<WarenausgangDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetWarenausgaengeAsync([FromQuery] WarenausgangFilterDto filter, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        _logger.LogInformation("GetWarenausgaengeAsync wurde aufgerufen mit dem Filter: {@Filter}, PageNumber: {PageNumber}, PageSize: {PageSize}", filter, pageNumber, pageSize);

        var query = new GetWarenausgangQuery
        {
            Filter = filter,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        return result.Match(
            success =>
            {
                _logger.LogInformation("Warenausgänge erfolgreich abgerufen.");
                return Ok(success);
            },
            error =>
            {
                _logger.LogError("Fehler beim Abrufen der Warenausgänge: {Error}", error);
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            });
    }
}

    // [HttpGet("{id:int}", Name = "GetWarenausgangById")] // Add a route name
    // [ProducesResponseType(typeof(WarenausgangDto), StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // public async Task<ActionResult<WarenausgangDto>> GetWarenausgangByIdAsync(int id)
    // {
    //     _logger.LogInformation($"GetWarenausgangByIdAsync wurde Aufgerufen mit der ID: {id}", id);
    //     var result = await _service.GetWarenausgangByIdAsync(id);
    //     if (result == null)
    //     {
    //         return NotFound();
    //     }
        
    //     return Ok(result);
    // }

//     [HttpPost]
//     [ProducesResponseType(typeof(WarenausgangDto), StatusCodes.Status201Created)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     public async Task<ActionResult<WarenausgangDto>> CreateWarenausgangAsync([FromBody] WarenausgangRequestDto dto)
//     {
//         try 
//         {


//         if (!ModelState.IsValid)
//         {
//             return BadRequest(ModelState);
//         }

//         var warenausgang = await _service.CreateWarenausgangAsync(dto);
//         if (warenausgang == null)
//         {
//             return BadRequest("Erstellung des Warenausgangs fehlgeschlagen.");
//         }

//         // Use CreatedAtRoute instead of CreatedAtAction
//         return CreatedAtRoute(
//             "GetWarenausgangById",          // Reference the named route
//             new { id = warenausgang.Id },  // Ensure the parameter matches the route definition
//             warenausgang                   // Return the created entity
//         );
//         }
//         catch (ArgumentException ex)
//         {
//             _logger.LogError(ex, "Fehler bei der Erstellung des Warenausgangs: {Message}", ex.Message);
//             return BadRequest(ex.Message);
//         }
//         catch (InvalidOperationException ex)
//         {
//             _logger.LogError(ex, "Fehler bei der Erstellung des Warenausgangs: {Message}", ex.Message);
//             return BadRequest(ex.Message);
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "Unbekannter Fehler bei der Erstellung des Warenausgangs: {Message}", ex.Message);
//             return StatusCode(StatusCodes.Status500InternalServerError, "Ein unbekannter Fehler ist aufgetreten.");
//         }
//     }

//     [HttpDelete("{id:int}")]
//     public async Task<IActionResult> DeleteWarenausgangAsync(int id)
//     {
//         var result = await _service.DeleteWarenausgangAsync(id);
//         if (!result)
//         {
//             return NotFound();
//         }
//         return NoContent();
//     }

//     [HttpGet("{id:int}/ArtikelPositionen")]
//     [ProducesResponseType(typeof(List<WarenausgangArtikelPositionenDto>), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status204NoContent)]
//     public async Task<ActionResult<List<WarenausgangArtikelPositionenDto>>> GetWarenausgangArtikelPositionenAsync(int id)
//     {
//         var result = await _service.GetWarenausgangArtikelPositionenByWarenausgangIdAsync(id);
//         if (result == null)
//         {
//             return NoContent();
//         }
//         return Ok(result);
//     }
// }
