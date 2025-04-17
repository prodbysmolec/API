using API.Common.Controllers;
using Application.Commands.Authentication;
using Application.Interfaces;
using Domain.Common.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using API.Handlers;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

public class AuthenticationController(
    ILogger<AuthenticationController> logger,
    IMediator mediator
) : BaseController
{
    private readonly ILogger<AuthenticationController> _logger = logger;
    private readonly IMediator _mediator = mediator;
    

    /// <summary>
    /// Registriert einen neuen Nutzer.
    /// </summary>
    /// <param name="command">Die parameter, welche fürs registrieren verwendet werden.</param>
    /// <returns></returns>
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        _logger.LogInformation("Versuche einen User mit der Email {Email} zu registrieren.", command.Email);
        var result = await _mediator.Send(command);

        return result.Match(
            success =>
            {  
                _logger.LogInformation("User mit der Email {Email} wurde erfolgreich registriert.", command.Email);
                return Ok(result.Value);
            },
            error =>
            {
                _logger.LogError("Failed to retrieve messages: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    /// <summary>
    /// Loggt einen Nutzer ein.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody]LoginCommand request)
    {
        _logger.LogInformation("Versuche einen User mit dem Usernamen {Username} einzuloggen.", request.Username);
        var response = await _mediator.Send(request);

        return response.Match(
            success =>
            {
                _logger.LogInformation("User mit dem Usernamen {Username} wurde erfolgreich eingeloggt.", request.Username);
                return Ok(response.Value);
            },
            error =>
            {
                _logger.LogError("Failed to retrieve messages: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    /// <summary>
    /// Prüft, ob der Token- und die Admin Rolle gültig sind.
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnlyEndpoint()
    {
        return Ok("du bist admin!");
    }

    /// <summary>
    /// Prüft, ob der Token gültig ist.
    /// Diese Methode ist für alle Nutzer zugänglich.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    public IActionResult AuthenticateOnlyEndpoint()
    {
        return Ok("du bist berechtigt!");
    }
}
