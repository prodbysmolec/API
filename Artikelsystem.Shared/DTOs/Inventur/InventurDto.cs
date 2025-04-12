using System;
using API.Features.Inventur.Models.Enums;

namespace Artikelsystem.Shared.DTOs.Inventur;


public class InventurDto
{
    public int Id { get; set; }
    public string? Bezeichnung { get; set; }
    public DateTime StartDatum { get; set; }
    public DateTime? AbschlussDatum { get; set; }
    public InventurStatus Status { get; set; }
    public string? Bemerkung { get; set; }

    // Zusammenfassung
    public int AnzahlArtikel { get; set; }
    public int AnzahlGeprueft { get; set; }
    public int AnzahlDifferenzen { get; set; }
    public decimal GesamtDifferenzWert { get; set; }

    // Positionen
    public List<InventurPositionDto> Positionen { get; set; } = new List<InventurPositionDto>();

    // Audit-Informationen
    public string? ErstelltVon { get; set; }
    public DateTime ErstelltAm { get; set; }
    public string? BearbeitetVon { get; set; }
    public DateTime BearbeitetAm { get; set; }
}