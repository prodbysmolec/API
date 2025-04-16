using System;
using API.Common.Controllers;
using API.Helfer;
using Application.Interfaces;
using Artikelsystem.Shared.DTOs.User.Request;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AuthenticationController(
    IAuthenticationService authenticationService,
    IAccessTokenHelper accessTokenHelper
) : BaseController
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly IAccessTokenHelper _accessTokenHelper = accessTokenHelper;

    [HttpPost("register")]
    public async Task<ActionResult> Register(UserDto request)
    {
        var user = await _authenticationService.RegisterAsync(request);

        if(user is null)
        {
            return BadRequest("Username already exists");
        }
        return Ok(user);
    }

}
