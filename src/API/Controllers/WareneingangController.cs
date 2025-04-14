using System;
using API.Common.Controllers;
using Application.Commands;
using Application.Queries.Wareneingaenge;
using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Request;
using Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Response;
using Domain.Common.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class WareneingangController(IMediator mediator, ILogger<WareneingangController> logger) : BaseController
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<WareneingangController> _logger = logger;

    /// <summary>
    /// Fügt eine neue Wareneingangsposition zu einem bestehenden Wareneingang hinzu.
    /// </summary>
    /// <param name="wareneingangId">WareneingangsID</param>
    /// <param name="request">ID des Wareneingangs, des Artikels, die Menge und der Einzelpreis</param>
    /// <returns></returns>
    [HttpPost("{wareneingangId:int}/positionen")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddWareneingangsPosition(
        int wareneingangId,
        [FromBody] AddWareneingangsPositionRequest request)
    {
    _logger.LogInformation("Füge neue Wareneingangsposition hinzu.");

    // Setze die WareneingangId in der Anfrage
    request.WareneingangId = wareneingangId;

    var result = await _mediator.Send(new AddWareneingangsPositionCommand(request));
    return result.Match(
        success =>
        {
            _logger.LogInformation("Wareneingangsposition erfolgreich hinzugefügt.");
            return Ok(new { message = "Wareneingangsposition erfolgreich hinzugefügt.", id = success });
        },
        error =>
        {
            _logger.LogError("Fehler beim Hinzufügen der Wareneingangsposition: {Error}", error);
            return StatusCode(StatusCodes.Status500InternalServerError, error);
        });
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDTO<GetAlleWareneingaengeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAlleWareneingaenge([FromQuery] int page = 1, [FromQuery] int recordsPerPage = 10)
    {
        _logger.LogInformation("Hole alle Wareneingänge mit Paging. Seite: {Page}, RecordsPerPage: {RecordsPerPage}", page, recordsPerPage);

        var result = await _mediator.Send(new GetAlleWareneingaengeQuery(page, recordsPerPage));
        return result.Match(
            success =>
            {
                _logger.LogInformation("Wareneingänge erfolgreich abgerufen.");
                return Ok(success);
            },
            error =>
            {
                _logger.LogError("Fehler beim Abrufen der Wareneingänge: {Error}", error);
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            });
    }
}
