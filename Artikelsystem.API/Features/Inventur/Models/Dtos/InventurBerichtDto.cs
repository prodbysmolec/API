using System;

namespace Artikelsystem.Api.Features.Inventur.Models.Dtos;

public class InventurBerichtDto
{
    public int Id { get; set; }
    public int InventurId { get; set; }
    public string Titel { get; set; } = string.Empty;
    public string Inhalt { get; set; } = string.Empty;
    public DateTime Erstellungsdatum { get; set; }

    public decimal GesamtDifferenzWert { get; set; }
    public int AnzahlPositionenMitDifferenz { get; set; }

    // Inventur Details
    public string? InventurBezeichnung { get; set; }
    public DateTime InventurStartDatum { get; set; }
    public DateTime? InventurAbschlussDatum { get; set; }

    // Audit-Informationen
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }
}
