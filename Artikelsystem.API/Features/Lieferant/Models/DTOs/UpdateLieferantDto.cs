namespace Artikelsystem.Api.Features.Lieferant.Models.DTOs;

public class UpdateLieferantRequest
{
    public string? Firma { get; set; }
    public string? Name { get; set; }
    public string? Vorname { get; set; }
    public string? EmailAdresse { get; set; }
    public string? Strasse { get; set; }
    public string? Hausnummer { get; set; }
    public string? PLZ { get; set; }
    public string? Ort { get; set; }
    public string? Telefonnummer { get; set; }
    public string? Notizen { get; set; }
}