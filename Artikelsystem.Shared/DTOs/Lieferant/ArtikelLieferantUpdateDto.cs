using System;

namespace Artikelsystem.Api.Features.Lieferant.Models.DTOs;

public class ArtikelLieferantUpdateDto
{
    public decimal Einkaufspreis { get; set; }
    public int Mindestbestellmenge { get; set; }
    public int Lieferzeit { get; set; }
    public string ArtikelNrBeimLieferanten { get; set; } = string.Empty;
    public bool IstPrimaer { get; set; }
}