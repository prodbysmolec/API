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
    public required WarenausgangZweckEnum Zweck { get; set; } = 0;

    // Standardfelder
    public required int Menge { get; set; }
    public string? Bemerkung { get; set; }
    public decimal? Verkaufspreis { get; set; }

    // Spezifische Referenzfelder je nach Zweck
    //public int? VeranstaltungsId { get; set; }
    public string? Rechnungsnummer { get; set; }
    public decimal? Gesamtpreis { get; set; }

    // Navigation Properties
    public virtual Warenausgaenge? Warenausgang { get; set; }
    public virtual Artikel.Models.Entitys.Artikel? Artikel { get; set; }
    // public virtual Veranstaltung? Veranstaltung { get; set; }
}