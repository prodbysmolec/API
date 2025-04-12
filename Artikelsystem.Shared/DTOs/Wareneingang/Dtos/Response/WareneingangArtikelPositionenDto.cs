using System;

namespace Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Response;

public class WareneingangArtikelPositionenDto
{
    public int Id { get; set; }
    public int ArtikelId { get; set; }
    public int WareneingangId { get; set; }
    public int Menge { get; set; }
    public decimal Einzelpreis { get; set; }
    public decimal Gesamtpreis { get; set; }
    public WareneingangDto? Wareneingang { get; set; }
}
