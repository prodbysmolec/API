using System;

namespace Artikelsystem.Shared.DTOs.Inventur;

public class InventurPositionDto
{
    public int Id { get; set; }
    public int ArtikelId { get; set; }
    public string ArtikelName { get; set; } = null!;
    public decimal ArtikelPreis { get; set; }

    public int SystemMenge { get; set; }
    public int? GezaehlteMenge { get; set; }
    public bool IstGeprueft { get; set; }

    public int? Differenz { get; set; }
    public decimal? DifferenzWert { get; set; }

    public string? Bemerkung { get; set; }

    // Audit-Informationen
    public string? ErstelltVon { get; set; }
    public DateTime ErstelltAm { get; set; }
    public string? BearbeitetVon { get; set; }
    public DateTime BearbeitetAm { get; set; }
}