using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Wareneingang;
using Domain.Entities.Lieferant;
using Domain.Entities.Warenausgang;
using Domain.Common;
using Artikelsystem.Shared.DTOs.Artikel.Enums;
using Domain.Entities.Warenausgang;
using Domain.Entities.Artikel;
namespace Domain.Entities.Artikel;
public class Artikel : AuditableEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Preis { get; set; }
    public required int Maximalbestand { get; set; }
    public required int Mindestbestand { get; set; }
    public int Menge { get; set; }
    public ArtikelStatus Status { get; set; } = ArtikelStatus.Verfügbar;
    public byte[] Bild { get; set; } = new byte[0];
    public bool HistorischGesetzt { get; set; } = false;
    public virtual ICollection<WareneingangArtikelPositionen> Wareneingaenge { get; set; } = new List<WareneingangArtikelPositionen>();
    public virtual ICollection<WarenausgangArtikelPositionen> Warenausgaenge { get; set; } = new List<WarenausgangArtikelPositionen>();
    public virtual ArtikelStatistik? ArtikelStatistik { get; set; }
    // Ergänzung für die Artikel-Klasse:
    public virtual ICollection<ArtikelZusatzWert> ArtikelZusatzWerte { get; set; } = new HashSet<ArtikelZusatzWert>();
    public virtual ICollection<ArtikelInventurHistorie> InventurHistorie { get; set; } = new HashSet<ArtikelInventurHistorie>();
    public virtual ICollection<ArtikelLieferant> ArtikelLieferanten { get; set; } = new List<ArtikelLieferant>();
}