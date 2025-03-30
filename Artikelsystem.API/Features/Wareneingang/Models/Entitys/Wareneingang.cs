using System;
using Artikelsystem.Api.Models;
using Artikelsystem.Api.Features.Lieferant.Models.Entitys;

namespace Artikelsystem.Api.Features.Wareneingang.Models.Entitys;

public class Wareneingang : AuditableEntity
{
    public int Id { get; set; }
    public int LieferantId { get; set; }
    
    public required int Menge { get; set; } = 0;
    public required decimal Gesamtpreis { get; set; }

    // Navigation properties
    public virtual ICollection<WareneingangArtikel> ArtikelPositionen { get; set; } = new List<WareneingangArtikel>();
    public virtual Lieferant.Models.Entitys.Lieferant? Lieferant { get; set; }
}