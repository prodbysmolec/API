using Artikelsystem.Api.Features.Artikel.Models.DTOs;
using Artikelsystem.Api.Features.Lieferant.Models.DTOs;

namespace Artikelsystem.Api.Features.Wareneingang.Models.DTOs;

public class WareneingangDto
{
    public int Id { get; set; }
    public int LieferantId { get; set; }
    public int Menge { get; set; }
    public decimal Gesamtpreis { get; set; }
    public DateTime? CreatedOn { get; set; }
    public LieferantDto? Lieferant { get; set; }
    public List<WareneingangArtikelDto> ArtikelPositionen { get; set; } = new List<WareneingangArtikelDto>();
}

public class WareneingangArtikelDto
{
    public int Id { get; set; }
    public int ArtikelId { get; set; }
    public int Menge { get; set; }
    public decimal Einzelpreis { get; set; }
    public decimal Gesamtpreis { get; set; }
    public ArtikelDto? Artikel { get; set; }
}