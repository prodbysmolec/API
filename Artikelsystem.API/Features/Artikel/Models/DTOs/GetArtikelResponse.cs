using System;
using Artikelsystem.Api.Features.Employees.Enums;
using Artikelsystem.Api.Features.Warenausgang.Models.DTOs.Responses;
using Artikelsystem.Api.Features.Wareneingang.Models.DTOs.Requests;
using Artikelsystem.Api.Features.Wareneingang.Models.Entitys;

namespace Artikelsystem.Api.Features.Artikel.Models.DTOs;


public class GetArtikelResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Preis { get; set; }
    public required int Maximalbestand { get; set; }
    public required int Mindestbestand { get; set; }
    public int Menge { get; set; }

    // Geändert zu ArtikelStatus Enum-Typ
    public ArtikelStatus Status { get; set; } = ArtikelStatus.Verfügbar;

    // Status als String für die Anzeige
    public string StatusName => Status.ToString();

    // Hinzugefügt für die Base64-Konvertierung
    public string? BildBase64 { get; set; }

    // Original Bild-Bytes beibehalten falls benötigt
    public byte[]? Bild { get; set; }

    // Berechnete Eigenschaften
    public bool IstUnterMindestbestand => Menge < Mindestbestand;
    public bool IstUeberMaximalbestand => Menge > Maximalbestand;

    // Optional: Statistik-Informationen
    public ArtikelStatistikDto? Statistik { get; set; }

    public List<WarenausgangArtikelPositionDto>? WarenausgangArtikelPosition { get; set; }
    public List<WareneingangArtikelPositionenDto>? WareneingangArtikelPosition { get; set; }



    public class ArtikelStatistikDto
    {
        public decimal Gesamtmenge { get; set; }
        public decimal DurchschnittlicherEinzelpreis { get; set; }
        public decimal DurchschnittlicherVerkaufspreis { get; set; }
        public int VerkaufsMenge { get; set; }
        public decimal Lagerwert { get; set; }
        public decimal GesamtVerkaufswert { get; set; }
    }
}