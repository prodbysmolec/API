using System;
using Artikelsystem.Domain.Entities.Artikel;
using Artikelsystem.Api.Features.Warenausgang.Models.Entitys;

namespace Artikelsystem.Domain.Entities.Warenausgang;

public class WarenausgangArtikelPositionen
{
    public int Id { get; set; }

    //Fremdschlüssel
    public int WarenausgangId { get; set; }
    public int ArtikelId { get; set; }

    // Standardfelder
    public required int Menge { get; set; } = 0;
    public string Bemerkung { get; set; } = "";
    public decimal? Verkaufspreis { get; set; }

    // Spezifische Referenzfelder je nach Zweck
    //public int? VeranstaltungsId { get; set; }
    public string? Rechnungsnummer { get; set; } = "";
    public decimal? Gesamtpreis { get; set; }

    // Navigation Properties
    public virtual Warenausgaenge Warenausgang { get; set; } = null!;
    public virtual Artikel.Artikel Artikel { get; set; } = new Artikel.Artikel() {
        Name = string.Empty,
        Maximalbestand = 0,
        Mindestbestand = 0,
    };
    // public virtual Veranstaltung? Veranstaltung { get; set; }
}