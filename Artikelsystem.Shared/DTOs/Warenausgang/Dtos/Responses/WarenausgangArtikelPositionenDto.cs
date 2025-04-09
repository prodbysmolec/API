using System;
using Artikelsystem.Shared.DTOs.Artikel.Response;
using Artikelsystem.Shared.DTOs.Warenausgang.Enums;

namespace Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;

public class WarenausgangArtikelPositionenDto
{
    public int Id { get; set; }
    public int WarenausgangId { get; set; }
    public int ArtikelId { get; set; }
    public string ArtikelName { get; set; } = "";
    public int Menge { get; set; }
    public string Bemerkung { get; set; } = "";
    public decimal? Verkaufspreis { get; set; }
    public string? Rechnungsnummer { get; set; } = "";
    public decimal? Gesamtpreis { get; set; }
    public WarenausgangDto? Warenausgang { get; set; }
    public ArtikelDto? Artikel { get; set; }
}
