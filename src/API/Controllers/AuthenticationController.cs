using System;
using API.Common.Controllers;
using API.Helfer;
using Application.Commands.Authentication;
using Application.Interfaces;
using Artikelsystem.Shared.DTOs.User.Request;
using Domain.Common.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using API.Handlers;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

public class AuthenticationController(
    IAuthenticationService authenticationService,
    ILogger<AuthenticationController> logger,
    IMediator mediator
) : BaseController
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly ILogger<AuthenticationController> _logger = logger;
    private readonly IMediator _mediator = mediator;
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);

        return result.Match(
            success =>
            {  
                return Ok(result.Value);
            },
            error =>
            {
                _logger.LogError("Failed to retrieve messages: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginCommand request)
    {
        var response = await _mediator.Send(request);

        return response.Match(
            success =>
            {
                return Ok(response.Value);
            },
            error =>
            {
                _logger.LogError("Failed to retrieve messages: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnlyEndpoint()
    {
        return Ok("du bist admin!");
    }

    [Authorize]
    [HttpGet]
    public IActionResult AuthenticateOnlyEndpoint()
    {
        return Ok("du bist berechtigt!");
    }
}
