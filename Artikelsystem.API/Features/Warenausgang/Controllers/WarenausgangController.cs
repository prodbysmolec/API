using System.Security.Claims;
using Artikelsystem.API.Features.Warenausgang.Service;
using Artikelsystem.API.Shared.Controllers;
using Artikelsystem.Shared;
using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Filter;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Request;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.AspNetCore;

namespace Artikelsystem.API.Features.Warenausgang.Controllers
{
    public class WarenausgangController : BaseController
    {
        private readonly ILogger<WarenausgangController> _logger;
        private readonly IWarenausgangService _service;

        public WarenausgangController(ILogger<WarenausgangController> logger, IWarenausgangService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDTO<WarenausgangDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<PagedResultDTO<WarenausgangDto>>> GetWarenausgaengeAsync([FromQuery] WarenausgangFilterDto filter, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            _logger.LogInformation($"GetWarenausgaengeAsync wurde Aufgerufen mit dem Filter: {@filter}, pageNumber: {pageNumber}, pageSize: {pageSize}", filter, pageNumber, pageSize);
            var result = await _service.GetWarenausgaengeAsync(filter, pageNumber, pageSize);
            if(result.Items == null || !result.Items.Any())
            {
                return NoContent();
            }
            return Ok(result);
        }

        [HttpGet("{id:int}", Name = "GetWarenausgangById")] // Add a route name
        [ProducesResponseType(typeof(WarenausgangDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WarenausgangDto>> GetWarenausgangByIdAsync(int id)
        {
            _logger.LogInformation($"GetWarenausgangByIdAsync wurde Aufgerufen mit der ID: {id}", id);
            var result = await _service.GetWarenausgangByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(WarenausgangDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WarenausgangDto>> CreateWarenausgangAsync([FromBody] WarenausgangRequestDto dto)
        {
            try 
            {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var warenausgang = await _service.CreateWarenausgangAsync(dto);
            if (warenausgang == null)
            {
                return BadRequest("Erstellung des Warenausgangs fehlgeschlagen.");
            }

            // Use CreatedAtRoute instead of CreatedAtAction
            return CreatedAtRoute(
                "GetWarenausgangById",          // Reference the named route
                new { id = warenausgang.Id },  // Ensure the parameter matches the route definition
                warenausgang                   // Return the created entity
            );
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Fehler bei der Erstellung des Warenausgangs: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Fehler bei der Erstellung des Warenausgangs: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unbekannter Fehler bei der Erstellung des Warenausgangs: {Message}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ein unbekannter Fehler ist aufgetreten.");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteWarenausgangAsync(int id)
        {
            var result = await _service.DeleteWarenausgangAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("{id:int}/ArtikelPositionen")]
        [ProducesResponseType(typeof(List<WarenausgangArtikelPositionenDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<WarenausgangArtikelPositionenDto>>> GetWarenausgangArtikelPositionenAsync(int id)
        {
            var result = await _service.GetWarenausgangArtikelPositionenByWarenausgangIdAsync(id);
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }
    }
}
