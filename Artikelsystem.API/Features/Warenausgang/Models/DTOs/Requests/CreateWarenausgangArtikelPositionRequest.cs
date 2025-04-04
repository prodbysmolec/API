using Artikelsystem.Api.Features.Warenausgang.Models.Entitys;

namespace Artikelsystem.Api.Features.Warenausgang.Models.DTOs.Requests;

public class CreateWarenausgangArtikelPositionRequest
{
    public required int ArtikelId { get; set; }
    public WarenausgangZweckEnum Zweck { get; set; }
    public int Menge { get; set; }
    public string? Bemerkung { get; set; }
    public decimal? Verkaufspreis { get; set; }
    public string? Rechnungsnummer { get; set; }
}