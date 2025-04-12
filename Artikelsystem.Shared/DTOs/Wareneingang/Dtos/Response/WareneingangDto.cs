using System;

namespace Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Response;

public class WareneingangDto
{
    public int Id { get; set; }
    public decimal Gesamtpreis { get; set; }
    public string AllgemeineBemerkungen { get; set; } = string.Empty;

    public List<WareneingangArtikelPositionenDto> WareneingangPositionen { get; set; } = new List<WareneingangArtikelPositionenDto>();
}