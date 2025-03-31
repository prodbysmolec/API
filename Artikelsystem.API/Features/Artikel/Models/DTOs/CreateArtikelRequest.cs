using Artikelsystem.Api.Features.Employees.Enums;
using Microsoft.AspNetCore.Http;

namespace Artikelsystem.Api.Features.Artikel.Models.DTOs;

public class CreateArtikelRequest
{
    public required string Name { get; set; }
    public decimal Preis { get; set; }
    public required int Mindestbestand { get; set; }
    public required int Maximalbestand { get; set; }
    public int Menge { get; set; } = 0;
    public ArtikelStatus Status { get; set; } = ArtikelStatus.Verfügbar;
    public IFormFile? BildFile { get; set; }
}