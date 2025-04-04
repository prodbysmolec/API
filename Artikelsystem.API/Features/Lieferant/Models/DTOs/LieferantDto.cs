namespace Artikelsystem.Api.Features.Lieferant.Models.DTOs;

public class LieferantDto
{
    public int Id { get; set; }
    public string Firma { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Vorname { get; set; } = string.Empty;
    public string EmailAdresse { get; set; } = string.Empty;
    public string Strasse { get; set; } = string.Empty;
    public string Hausnummer { get; set; } = string.Empty;
    public string PLZ { get; set; } = string.Empty;
    public string Ort { get; set; } = string.Empty;
    public string Telefonnummer { get; set; } = string.Empty;
    public string? Notizen { get; set; }
}