using System;

namespace Artikelsystem.Api.Features.Wareneingang.Models.DTOs.Requests;

public class WareneingangDto
{
    public int Id { get; set; }
    public decimal Gesamtpreis { get; set; }
    public string AllgemeineBemerkungen { get; set; } = string.Empty;

    public List<WareneingangArtikelPositionenDto> WareneingangsPositionen { get; set; } = new List<WareneingangArtikelPositionenDto>();
}