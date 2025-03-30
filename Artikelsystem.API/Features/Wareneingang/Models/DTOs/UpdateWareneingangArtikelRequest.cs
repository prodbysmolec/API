namespace Artikelsystem.Api.Features.Wareneingang.Models.DTOs;

public class UpdateWareneingangArtikelRequest
{
    public int Id { get; set; } // 0 für neue Positionen
    public int ArtikelId { get; set; }
    public int Menge { get; set; }
    public decimal Einzelpreis { get; set; }
}