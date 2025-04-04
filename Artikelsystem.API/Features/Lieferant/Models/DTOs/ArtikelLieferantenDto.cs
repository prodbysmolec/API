using System;

namespace Artikelsystem.Api.Features.Lieferant.Models.DTOs;


public class ArtikelLieferantDto
{
    public int Id { get; set; }
    public int ArtikelId { get; set; }
    public int LieferantId { get; set; }

    // Lieferantendaten
    public string LieferantFirma { get; set; } = string.Empty;
    public string LieferantName { get; set; } = string.Empty;
    public string LieferantVorname { get; set; } = string.Empty;

    // Preis- und Bestellinformationen
    public decimal Einkaufspreis { get; set; }
    public int? Mindestbestellmenge { get; set; }
    public int? Lieferzeit { get; set; }
    public string? ArtikelNrBeimLieferanten { get; set; }

    // Status
    public bool IstAktiv { get; set; }
    public bool IstPrimaerLieferant { get; set; }
    public DateTime? GueltigVon { get; set; }
    public DateTime? GueltigBis { get; set; }
    public string ArtikelName { get; set; } = string.Empty;

    // Zeitraum formatiert für die Anzeige
    public string ZeitraumText => IstAktiv
        ? $"Seit {GueltigVon:dd.MM.yyyy}"
        : $"Von {GueltigVon:dd.MM.yyyy} bis {GueltigBis:dd.MM.yyyy}";
}
