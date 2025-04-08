using System;
using Artikelsystem.Shared.DTOs.Warenausgang.Enums;

namespace Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;

public class WarenausgangDto
{
    public int Id { get; set; }
    public string? AllgemeineBemerkungen { get; set; }
    public required WarenausgangZweckEnum Zweck { get; set; } = 0;
    public DateTime ErstelltAm { get; set; }
    public DateTime BearbeitetAm { get; set; }
    public string? ErstelltVon { get; set; } = "";
    public string? BearbeitetVon { get; set; } = "";
    public virtual List<WarenausgangArtikelPositionenDto> ArtikelPositionen { get; set; } = new List<WarenausgangArtikelPositionenDto>();
}
