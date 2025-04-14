using System;
using Artikelsystem.Shared.DTOs.Artikel.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace Artikelsystem.Shared.DTOs.Artikel.Request;

public class CreateArtikelRequest
{
    public required string Name { get; set; }
    public decimal Preis { get; set; }
    public required int Mindestbestand { get; set; }
    public required int Maximalbestand { get; set; }
    public int Menge { get; set; } = 0;
    public ArtikelStatus Status { get; set; } = ArtikelStatus.Verfügbar;
    public IFormFile? Bild { get; set;}
    public required int ArtikelGruppeId { get; set; } 
}


