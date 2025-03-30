namespace Artikelsystem.Api.Features.Lieferant.Models.DTOs;

public class CreateLieferantRequest
{
    public required string Firma { get; set; }
    public required string Name { get; set; }
    public required string Vorname { get; set; }
    public required string EmailAdresse { get; set; }
    public required string Strasse { get; set; }
    public required string Hausnummer { get; set; }
    public required string PLZ { get; set; }
    public required string Ort { get; set; }
    public required string Telefonnummer { get; set; }
    public string? Notizen { get; set; }
}