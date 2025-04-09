using Artikelsystem.Domain.Entities.Artikel;

namespace Artikelsystem.Api.Features.Wareneingang.Models.Entitys;

public class WareneingangArtikelPositionen
{
    public int Id { get; set; }
    public int ArtikelId { get; set; }
    public int WareneingangId { get; set; }
    public int Menge { get; set; }

    public decimal Einzelpreis { get; set; }

    // Berechnete Eigenschaften
    // Gesamtpreis = Menge * Einzelpreis
    private decimal _GesamtPreis;
    public decimal Gesamtpreis
    {
        get { return _GesamtPreis; }
        set { _GesamtPreis = value; }
    }

    // Navigation properties
    public virtual Artikel? Artikel { get; set; }  // Verwende den Alias hier
    public virtual Wareneingaenge? Wareneingang { get; set; }
}