using System;

namespace Artikelsystem.Shared.DTOs.Artikel.Response;

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