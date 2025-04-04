using System;
using Artikelsystem.Api.Features.Warenausgang.Models.Entitys;

namespace Artikelsystem.Api.Features.Warenausgang.Models.DTOs.Requests;

public class UpdateWarenausgangArtikelPositionRequest
{
    public int? Id { get; set; }
    public required int ArtikelId { get; set; }
    public required WarenausgangZweckEnum Zweck { get; set; } = 0;
    public required int Menge { get; set; }
    public string? Bemerkung { get; set; }
    public decimal? Verkaufspreis { get; set; }
    public string? Rechnungsnummer { get; set; }
}
