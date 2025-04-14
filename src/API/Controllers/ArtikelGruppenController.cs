using System;
using API.Common.Controllers;
using Application.Queries.ArtikelGruppe;
using Artikelsystem.Shared.DTOs.ArtikelGruppe.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Domain.Common.ResultPattern;
using Artikelsystem.Shared.Helfer;


namespace API.Controllers;

public class ArtikelGruppenController(IMediator mediator, ILogger<ArtikelGruppenController> logger) : BaseController
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<ArtikelGruppenController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAllArtikelGruppen([FromQuery] FilteringDTO query)
    {
        _logger.LogInformation("GetAllArtikelGruppen called");
        var result = await _mediator.Send(new ArtikelGruppenQuery(query));
        return result.Match(
            success =>
            {
                _logger.LogInformation("Alle Artikel erfolgreich abgerufen.");
                return Ok(success);
            },
            error =>
            {
                _logger.LogError("Fehler beim Abrufen der Artikel: {Error}", error);
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            });
    }
}
