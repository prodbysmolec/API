using System;
using Artikelsystem.Domain.Common;
using Artikelsystem.Domain.Entities.Artikel;

namespace Artikelsystem.Domain.Entities.Inventur;

public class InventurPosition : AuditableEntity
{
    public int Id { get; set; }
    public int InventurId { get; set; }
    public int ArtikelId { get; set; }

    // Werte aus dem System vor der Inventur
    public int Menge { get; set; }

    // Werte nach der Zählung
    public int? GezaehlteMenge { get; set; }
    public bool IstGeprueft { get; set; } = false;

    // GezaehlteMenge (Neu-Bestand) - Systemmenge (Alt-Bestand)
    #region Berechnete Felder
    // GezaehlteMenge.HasValue ? GezaehlteMenge.Value - Menge : null;
    public int Differenz { get; set; }
    public decimal? DifferenzWert { get; set; }
    #endregion

    public string? Bemerkung { get; set; }

    // Navigation Propertys
    public virtual Inventur Inventur { get; set; } = null!;
    public virtual Artikel.Artikel Artikel { get; set; } = null!;
}
