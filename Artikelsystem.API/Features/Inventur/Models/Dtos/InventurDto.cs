using System;
using Artikelsystem.Api.Features.Inventur.Models.Enums;

namespace Artikelsystem.Api.Features.Inventur.Models.Dtos;


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
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }
}