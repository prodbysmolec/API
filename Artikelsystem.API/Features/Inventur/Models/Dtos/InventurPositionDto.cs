using System;

namespace Artikelsystem.Api.Features.Inventur.Models.Dtos;

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
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }
}