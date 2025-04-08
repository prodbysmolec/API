using System;
using Artikelsystem.Shared.DTOs.User.Request;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Artikelsystem.API.Shared.Controllers;

[ApiController]
[Route("/[controller]")]
[Produces("application/json")]
public abstract class BaseController : Controller
{

}