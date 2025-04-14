using System;
using Artikelsystem.Shared.DTOs.User.Request;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Domain.Common.ResultPattern;
namespace API.Common.Controllers;

[ApiController]
[Route("/[controller]")]
[Produces("application/json")]
public abstract class BaseController : Microsoft.AspNetCore.Mvc.Controller
{

    // protected string GetCurrentUserId()
    // {
    //     return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User is not authenticated.");
    // }

    // protected string GetCurrentUserName()
    // {
    //     return User.Identity?.Name ?? throw new UnauthorizedAccessException("User is not authenticated.");
    // }

    // protected string GetAccessToken()
    // {
    //     var authorizationHeader = Request.Headers["Authorization"].ToString();
    //     if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
    //     {
    //         throw new UnauthorizedAccessException("Access token is missing or invalid.");
    //     }

    //     return authorizationHeader.Substring("Bearer ".Length).Trim();
    // }
}