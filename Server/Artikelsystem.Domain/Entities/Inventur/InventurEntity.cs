using System;

namespace Artikelsystem.Domain.Entities.Inventur;

public class InventurEntity
{
    public int Id { get; set; }
    public int InventurId { get; set; }
    public string Titel { get; set; } = string.Empty;
    public string Inhalt { get; set; } = string.Empty;
    public DateTime Erstellungsdatum { get; set; }
    public decimal GesamtDifferenzWert { get; set; }
    public int AnzahlPositionenMitDifferenz { get; set; }

    // Navigation Propertys
    public virtual Inventur Inventur { get; set; } = null!;
}
