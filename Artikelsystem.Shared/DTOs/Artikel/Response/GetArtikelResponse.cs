using System;
using Artikelsystem.Shared.DTOs.Artikel.Enums;
using Artikelsystem.Shared.DTOs.Artikel.Response;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;
using Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Response;

namespace Artikelsystem.Shared.DTOs.Artikel.Request;

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
    public virtual ArtikelStatistikDto? Statistik { get; set; }

    public List<WarenausgangArtikelPositionenDto>? WarenausgangArtikelPosition { get; set; }
    public List<WareneingangArtikelPositionenDto>? WareneingangArtikelPosition { get; set; }

}
