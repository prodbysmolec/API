using Artikelsystem.Api.Features.Artikel.Models.DTOs;
using Artikelsystem.Api.Features.Artikel.Services;
using Artikelsystem.Api.Features.Employees.Enums;
using Artikelsystem.API.Shared.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Artikelsystem.Api.Features.Artikel.Controllers;

public class ArtikelController : BaseController
{
    private readonly IArtikelService _artikelService;
    private readonly ILogger<ArtikelController> _logger;

    public ArtikelController(
        IArtikelService artikelService,
        ILogger<ArtikelController> logger)
    {
        _artikelService = artikelService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all articles in the system.
    /// </summary>
    /// <returns>Returns the articles in a JSON array.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ArtikelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllArtikel()
    {
        _logger.LogInformation("Getting all articles");
        try
        {
            var artikel = await _artikelService.GetAllArtikelAsync();
            return Ok(artikel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all articles");
            return StatusCode(500, "An error occurred while retrieving articles");
        }
    }

    /// <summary>
    /// Gets an article by ID.
    /// </summary>
    /// <param name="id">The ID of the article.</param>
    /// <returns>The single Article record.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ArtikelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetArtikelById(int id)
    {
        _logger.LogInformation("Getting article with ID: {ArtikelId}", id);
        try
        {
            var artikel = await _artikelService.GetArtikelByIdAsync(id);
            if (artikel == null)
            {
                _logger.LogWarning("Article with ID: {ArtikelId} not found", id);
                return NotFound();
            }

            return Ok(artikel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving article with ID: {ArtikelId}", id);
            return StatusCode(500, "An error occurred while retrieving the article");
        }
    }

    /// <summary>
    /// Creates an article.
    /// </summary>
    /// <param name="request">The article data to create.</param>
    /// <returns>The created article.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ArtikelDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateArtikel([FromForm] CreateArtikelRequest request)
    {
        _logger.LogInformation("Creating new article with name: {ArtikelName}", request.Name);
        try
        {
            var artikel = await _artikelService.CreateArtikelAsync(request);
            _logger.LogInformation("Article created successfully with ID: {ArtikelId}", artikel.Id);
            return CreatedAtAction(nameof(GetArtikelById), new { id = artikel.Id }, artikel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating article with name: {ArtikelName}", request.Name);
            return StatusCode(500, "An error occurred while creating the article");
        }
    }

    /// <summary>
    /// Updates an article.
    /// </summary>
    /// <param name="id">The ID of the article to update.</param>
    /// <param name="request">The article data to update.</param>
    /// <returns>The updated article.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ArtikelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateArtikel(int id, [FromForm] UpdateArtikelRequest request)
    {
        _logger.LogInformation("Updating article with ID: {ArtikelId}", id);
        try
        {
            var artikel = await _artikelService.UpdateArtikelAsync(id, request);
            if (artikel == null)
            {
                _logger.LogWarning("Article with ID: {ArtikelId} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Article with ID: {ArtikelId} successfully updated", id);
            return Ok(artikel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating article with ID: {ArtikelId}", id);
            return StatusCode(500, "An error occurred while updating the article");
        }
    }

    /// <summary>
    /// Deletes an article.
    /// </summary>
    /// <param name="id">The ID of the article to delete.</param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteArtikel(int id)
    {
        _logger.LogInformation("Deleting article with ID: {ArtikelId}", id);
        try
        {
            var result = await _artikelService.DeleteArtikelAsync(id);
            if (!result)
            {
                _logger.LogWarning("Article with ID: {ArtikelId} not found or cannot be deleted", id);
                return NotFound();
            }

            _logger.LogInformation("Article with ID: {ArtikelId} successfully deleted", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting article with ID: {ArtikelId}", id);
            return StatusCode(500, "An error occurred while deleting the article");
        }
    }

    /// <summary>
    /// Gets articles with stock below minimum threshold.
    /// </summary>
    /// <returns>A list of articles with low stock.</returns>
    [HttpGet("low-stock")]
    [ProducesResponseType(typeof(IEnumerable<ArtikelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetArtikelMitBestandUnterMindestbestand()
    {
        _logger.LogInformation("Getting articles with stock below minimum threshold");
        try
        {
            var artikel = await _artikelService.GetArtikelMitBestandUnterMindestbestandAsync();
            return Ok(artikel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving articles with low stock");
            return StatusCode(500, "An error occurred while retrieving articles with low stock");
        }
    }

    /// <summary>
    /// Updates the stock quantity of an article.
    /// </summary>
    /// <param name="id">The ID of the article.</param>
    /// <param name="menge">The quantity to add (positive) or subtract (negative).</param>
    /// <returns></returns>
    [HttpPatch("{id:int}/bestand")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateArtikelBestand(int id, [FromBody] int menge)
    {
        _logger.LogInformation("Updating stock for article with ID: {ArtikelId}, change by: {Menge}", id, menge);
        try
        {
            var result = await _artikelService.UpdateArtikelBestandAsync(id, menge);
            if (!result)
            {
                _logger.LogWarning("Article with ID: {ArtikelId} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Stock for article with ID: {ArtikelId} successfully updated", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating stock for article with ID: {ArtikelId}", id);
            return StatusCode(500, "An error occurred while updating the article stock");
        }
    }

    /// <summary>
    /// Updates the status of an article.
    /// </summary>
    /// <param name="id">The ID of the article.</param>
    /// <param name="status">The new status value.</param>
    /// <returns></returns>
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateArtikelStatus(int id, [FromBody] ArtikelStatus status)
    {
        _logger.LogInformation("Updating status for article with ID: {ArtikelId} to {Status}", id, status);
        try
        {
            var result = await _artikelService.UpdateArtikelStatusAsync(id, status);
            if (!result)
            {
                _logger.LogWarning("Article with ID: {ArtikelId} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Status for article with ID: {ArtikelId} successfully updated to {Status}", id, status);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating status for article with ID: {ArtikelId}", id);
            return StatusCode(500, "An error occurred while updating the article status");
        }
    }

    /// <summary>
    /// Updates article statistics.
    /// </summary>
    /// <param name="id">The ID of the article.</param>
    /// <returns></returns>
    [HttpPost("{id:int}/aktualisiere-statistik")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AktualisiereArtikelStatistik(int id)
    {
        _logger.LogInformation("Updating statistics for article with ID: {ArtikelId}", id);
        try
        {
            var result = await _artikelService.AktualisiereArtikelStatistikAsync(id);
            if (!result)
            {
                _logger.LogWarning("Article with ID: {ArtikelId} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Statistics for article with ID: {ArtikelId} successfully updated", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating statistics for article with ID: {ArtikelId}", id);
            return StatusCode(500, "An error occurred while updating the article statistics");
        }
    }
}