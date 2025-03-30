using System;
using Artikelsystem.Api.Features.Wareneingang.Models.Entitys;

namespace Artikelsystem.Api.Features.Lieferant.Models.Entitys;

public class Lieferant
{
    public int Id { get; set; }
    public required string Firma { get; set;}
    public required string Name { get; set; }
    public required string Vorname { get; set; }
    public required string EmailAdresse { get; set; }
    public required string Strasse { get; set; }
    public required string Hausnummer { get; set; }
    public required string PLZ { get; set; }
    public required string Ort { get; set; }
    public required string Telefonnummer { get; set; }
    public string? Notizen { get; set; }

    public virtual ICollection<Wareneingang.Models.Entitys.Wareneingang> Wareneingaenge { get; set; } = new List<Wareneingang.Models.Entitys.Wareneingang>();

}
