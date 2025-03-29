using System;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1;

[ApiController]
[Route("/[controller]")]
public abstract class BaseController : Controller
{
}