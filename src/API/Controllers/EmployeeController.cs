using System;
using API.Common.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Application.Queries.Employee;
using Domain.Common.ResultPattern;
using Application.DTOs.Employee;
using Application.Queries;
using Application.Commands;
namespace API.Controllers;

public class EmployeeController(IMediator mediator, ILogger<EmployeeController> logger) : BaseController
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<EmployeeController> _logger = logger;

    /// <summary>
    /// Ruft alle Employees in der Datenbank ab.
    /// </summary>
    /// <returns>Returnt alle Employees in einem JSON Array.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        _logger.LogInformation("Alle Employees abrufen.");
        var result = await _mediator.Send(new GetEmployeesQuery());
        return result.Match(
            success => 
            {
                _logger.LogInformation("Alle Employees erfolgreich abgerufen.");
                return Ok(success);
            },
            error => 
            {
                _logger.LogError("Fehler beim Abrufen der Employees: {Error}", error);
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            });
    }

    /// <summary>
    /// Ruft einen Employee anhand der ID ab.
    /// </summary>
    /// <param name="id">Die ID des Employees.</param>
    /// <returns>Der Employee oder ein Fehler.</returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        _logger.LogInformation("Employee mit ID {Id} abrufen.", id);
        var result = await _mediator.Send(new GetEmployeeQuery(id));
        return result.Match(
            success =>
            {
                _logger.LogInformation("Employee mit ID {Id} erfolgreich abgerufen.", id);
                return Ok(success);
            },
            error =>
            {
                _logger.LogError("Fehler beim Abrufen des Employees mit ID {Id}: {Error}", id, error);
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            });
    }

    /// <summary>
    /// Erstellt einen neuen Employee.
    /// </summary>
    /// <param name="command">Die Daten des neuen Employees.</param>
    /// <returns>Die ID des erstellten Employees oder ein Fehler.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeCommand command)
    {
        _logger.LogInformation("Neuen Employee erstellen.");
        var result = await _mediator.Send(command);
        return result.Match(
            success =>
            {
                _logger.LogInformation("Employee erfolgreich erstellt mit ID {Id}.", success);
                return Ok(new { message = "Employee erfolgreich hinzugefügt." });
            },
            error =>
            {
                _logger.LogError("Fehler beim Erstellen des Employees: {Error}", error);
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            });
    }

    
}
