using System;

namespace Artikelsystem.Shared.DTOs.Lieferant;

public class LieferantArtikelDto
{
    public int ArtikelId { get; set; }
    public string ArtikelName { get; set; } = string.Empty;
    public decimal Einkaufspreis { get; set; }
    public bool IstPrimaerLieferant { get; set; }
    public string? ArtikelNrBeimLieferanten { get; set; }
    public DateTime? SeitDatum { get; set; }
}