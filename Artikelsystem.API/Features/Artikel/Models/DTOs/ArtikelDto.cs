using Artikelsystem.Api.Features.Employees.Enums;

namespace Artikelsystem.Api.Features.Artikel.Models.DTOs;

public class ArtikelDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Preis { get; set; }
    public int Mindestbestand { get; set; }
    public int Maximalbestand { get; set; }
    public int Menge { get; set; }
    public ArtikelStatus Status { get; set; }
    public byte[]? Bild { get; set; }
    public ArtikelStatistikDto? ArtikelStatistik { get; set; }
}

public class ArtikelStatistikDto
{
    public int Id { get; set; }
    public int ArtikelId { get; set; }
    public decimal Gesamtmenge { get; set; }
    public decimal DurchschnittlicherEinzelpreis { get; set; }
    public decimal DurchschnittlicherVerkaufspreis { get; set; }
    public int VerkaufsMenge { get; set; }
    public decimal Lagerwert { get; set; }
    public decimal GesamtVerkaufswert { get; set; }
}