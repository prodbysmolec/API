using Artikelsystem.Api.Features.Employees.Enums;
using Microsoft.AspNetCore.Http;

namespace Artikelsystem.Api.Features.Artikel.Models.DTOs;

public class UpdateArtikelRequest
{
    public string? Name { get; set; }
    public decimal? Preis { get; set; }
    public int? Mindestbestand { get; set; }
    public int? Maximalbestand { get; set; }
    public int? Menge { get; set; }
    public ArtikelStatus? Status { get; set; }
    public IFormFile? BildFile { get; set; }
    public bool EntferneBild { get; set; } = false;
}