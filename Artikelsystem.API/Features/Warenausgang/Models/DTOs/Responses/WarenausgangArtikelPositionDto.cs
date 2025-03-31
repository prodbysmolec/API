using System;
using Artikelsystem.Api.Features.Warenausgang.Models.Entitys;

namespace Artikelsystem.Api.Features.Warenausgang.Models.DTOs.Responses;

    public class WarenausgangArtikelPositionDto
    {
    public int Id { get; set; }
    public int WarenausgangId { get; set; }
    public int ArtikelId { get; set; }
    public string ArtikelName { get; set; } = "";
    public WarenausgangZweckEnum Zweck { get; set; } = 0;
    public string ZweckBezeichnung { get; set; } = "";
    public int Menge { get; set; }
    public string Bemerkung { get; set; } = "";
    public decimal? Verkaufspreis { get; set; }
    public string Rechnungsnummer { get; set; } = "";
    public decimal? Gesamtpreis { get; set; }
}
