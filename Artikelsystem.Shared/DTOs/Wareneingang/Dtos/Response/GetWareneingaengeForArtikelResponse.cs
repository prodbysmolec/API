using System;

namespace Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Response;

public class GetWareneingaengeForArtikelResponse
{
    public int WareneingangId { get; set; }
    public DateTime Datum { get; set; }
    public decimal Gesamtpreis { get; set; }
    public string? AllgemeineBemerkungen { get; set; }
    public List<WareneingangArtikelPositionenDto> ArtikelPositionen { get; set; } = new();
}

public class WareneingangArtikelPositionenDto
{
    public int ArtikelId { get; set; }
    public string ArtikelName { get; set; } = string.Empty;
    public int Menge { get; set; }
    public decimal Einzelpreis { get; set; }
    public decimal Gesamtpreis => Menge * Einzelpreis; // Berechneter Wert
}