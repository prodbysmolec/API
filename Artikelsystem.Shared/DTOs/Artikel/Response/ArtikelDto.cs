using System;
using Artikelsystem.Shared.DTOs.Artikel.Enums;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;
using Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Response;

namespace Artikelsystem.Shared.DTOs.Artikel.Response;

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
    public List<WareneingangArtikelPositionenDto>? WareneingangArtikelPositionen { get; set; }
    public List<WarenausgangArtikelPositionenDto>? WarenausgangArtikelPositionen { get; set; }
}
