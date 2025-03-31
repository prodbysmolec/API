namespace Artikelsystem.Api.Features.Artikel.Models.DTOs;

public class GetAllArtikelRequest
{
    // Pagination
    public int? Page { get; set; }
    public int? RecordsPerPage { get; set; }
    
    // Name Filter
    public string? NameContains { get; set; }
    
    // Preis Filter
    public decimal? MinPreis { get; set; }
    public decimal? MaxPreis { get; set; }
    
    // Bestand Filter
    public int? MinMenge { get; set; }
    public int? MaxMenge { get; set; }
    
    // Status Filter
    public int? StatusId { get; set; }
    
    // Bestand Vergleich zu Min/Max
    public bool? UnterMindestbestand { get; set; }
    public bool? UeberMaximalbestand { get; set; }
    
    // Statistik Filter
    public decimal? MinDurchschnittlicherEinzelpreis { get; set; }
    public decimal? MaxDurchschnittlicherEinzelpreis { get; set; }
    public decimal? MinLagerwert { get; set; }
    public decimal? MaxLagerwert { get; set; }
    
    // Sortierung
    public string? SortBy { get; set; }
    public bool? SortDesc { get; set; }
}