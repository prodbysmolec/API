using System;
using Artikelsystem.Domain.Common;
using Artikelsystem.Domain.Entities.Artikel;

namespace Artikelsystem.Api.Features.Lieferant.Models.Entitys;

public class ArtikelLieferant : AuditableEntity
{
    public int Id { get; set; }

    public int ArtikelId { get; set; }
    public int LieferantId { get; set; }

    // Preisinformationen
    public decimal Einkaufspreis { get; set; }

    // Bestellinformationen
    public int? Mindestbestellmenge { get; set; }
    public int? Lieferzeit { get; set; }  // in Tagen

    // Artikel- und Lieferanten-spezifische Informationen
    public string? ArtikelNrBeimLieferanten { get; set; }

    // Status Flags
    public bool IstAktiv { get; set; } = true;
    public bool IstPrimaerLieferant { get; set; } = false;
    public DateTime? GueltigVon { get; set; }
    public DateTime? GueltigBis { get; set; }

    // Navigation Properties
    public virtual Artikel? Artikel { get; set; }
    public virtual Lieferant? Lieferant { get; set; }
}
