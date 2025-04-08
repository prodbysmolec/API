using System;
using Artikelsystem.Api.Features.Artikel.Models.Entitys;
using Microsoft.AspNetCore.Antiforgery;


namespace Artikelsystem.Api.Features.Warenausgang.Models.Entitys;

/// <summary>
/// Die Artikel Positionen eines Warenausgangs.
/// </summary>
public class WarenausgangArtikelPositionen
{
    public int Id { get; set; }

    //Fremdschlüssel
    public int WarenausgangId { get; set; }
    public int ArtikelId { get; set; }

    // Standardfelder
    public required int Menge { get; set; }
    public string Bemerkung { get; set; } = string.Empty;
    public decimal? Verkaufspreis { get; set; }

    // Spezifische Referenzfelder je nach Zweck
    //public int? VeranstaltungsId { get; set; }
    public string Rechnungsnummer { get; set; } = string.Empty;
    public decimal? Gesamtpreis { get; set; }

    // Navigation Properties
    public virtual Warenausgaenge Warenausgang { get; set; } = null!;
    public virtual Artikel.Models.Entitys.Artikel Artikel { get; set; } = new Artikel.Models.Entitys.Artikel() {
        Name = string.Empty,
        Maximalbestand = 0,
        Mindestbestand = 0,
    };
    // public virtual Veranstaltung? Veranstaltung { get; set; }
}