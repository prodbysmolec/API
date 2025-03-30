namespace Artikelsystem.Api.Features.Wareneingang.Models.DTOs;

public class CreateWareneingangRequest
{
    public int LieferantId { get; set; }
    public List<CreateWareneingangArtikelRequest> ArtikelPositionen { get; set; } = new List<CreateWareneingangArtikelRequest>();
}

public class CreateWareneingangArtikelRequest
{
    public int ArtikelId { get; set; }
    public int Menge { get; set; }
    public decimal Einzelpreis { get; set; }
}