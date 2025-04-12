using System;

namespace Artikelsystem.Shared.DTOs.Lieferant;

public class ArtikelLieferantUpdateDto
{
    public decimal Einkaufspreis { get; set; }
    public int Mindestbestellmenge { get; set; }
    public int Lieferzeit { get; set; }
    public string ArtikelNrBeimLieferanten { get; set; } = string.Empty;
    public bool IstPrimaer { get; set; }
}