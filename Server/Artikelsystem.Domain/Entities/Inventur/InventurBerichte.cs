using System;
using Artikelsystem.Domain.Common;

namespace Artikelsystem.Domain.Entities.Inventur;

public class InventurBerichte : AuditableEntity
{
    public int Id { get; set; }
    public int InventurId { get; set; }

    public string Titel { get; set; } = string.Empty;
    public string Inhalt { get; set; } = string.Empty;
    public DateTime Erstellungsdatum { get; set; } = DateTime.UtcNow;

    public decimal GesamtDifferenzWert { get; set; }
    public int AnzahlPositionenMitDifferenz { get; set; }

    // Navigation Property
    public virtual Inventur Inventur { get; set; } = null!;
}
