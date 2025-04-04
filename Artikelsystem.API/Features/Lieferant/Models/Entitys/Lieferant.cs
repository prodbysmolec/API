using System;
using Artikelsystem.Api.Features.Wareneingang.Models.Entitys;

namespace Artikelsystem.Api.Features.Lieferant.Models.Entitys;

public class Lieferant
{
    public int Id { get; set; }
    public required string Firma { get; set; }
    public required string Name { get; set; }
    public required string Vorname { get; set; }
    public required string EmailAdresse { get; set; }
    public required string Strasse { get; set; }
    public required string Hausnummer { get; set; }
    public required string PLZ { get; set; }
    public required string Ort { get; set; }
    public required string Telefonnummer { get; set; }
    public string? Notizen { get; set; }
    public bool IstAktiv { get; set; } = true;

    // Navigation property für die Artikel-Lieferant Beziehungen
    public virtual ICollection<ArtikelLieferant> ArtikelLieferanten { get; set; } = new List<ArtikelLieferant>();
}
