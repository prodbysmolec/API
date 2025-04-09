using Artikelsystem.Shared.DTOs.Warenausgang.Enums;

namespace Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Request;

public class CreateWarenausgangDto
{
    public WarenausgangZweckEnum Zweck { get; set; }
    public string? AllgemeineBemerkungen { get; set; }
    public string ErstelltVon { get; set; } = string.Empty;
    public List<CreateWarenausgangArtikelPositionDto> ArtikelPositionen { get; set; } = new();
}

public class CreateWarenausgangArtikelPositionDto
{
    public int ArtikelId { get; set; }
    public int Menge { get; set; }
    public string Bemerkung { get; set; } = string.Empty;
    public decimal? Verkaufspreis { get; set; }
    public string Rechnungsnummer { get; set; } = string.Empty;
}
