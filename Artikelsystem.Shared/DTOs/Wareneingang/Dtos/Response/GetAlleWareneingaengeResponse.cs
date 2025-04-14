using System;

namespace Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Response;

public class GetAlleWareneingaengeResponse
{
    public int WareneingangId { get; set; }
    public DateTime Datum { get; set; }
    public decimal Gesamtpreis { get; set; }
    public string? AllgemeineBemerkungen { get; set; }
    public List<WareneingangArtikelPositionenDto> ArtikelPositionen { get; set; } = new();
}
