using System;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Artikelsystem.API.Shared.Controllers;

[ApiController]
[Route("/[controller]")]
[Produces("application/json")]
public abstract class BaseController : Controller
{
}