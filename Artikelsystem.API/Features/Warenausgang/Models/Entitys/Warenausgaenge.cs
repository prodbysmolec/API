using System;
using Artikelsystem.Api.Models;

namespace Artikelsystem.Api.Features.Warenausgang.Models.Entitys;

public class Warenausgaenge : AuditableEntity
{
    public int Id { get; set; }
    public required string Mitarbeiter { get; set; }

    // Allgemeine Bemerkungen zum Warenausgang
    public string? AllgemeineBemerkungen { get; set; }
    // Navigation Property
    public virtual ICollection<WarenausgangArtikelPositionen> ArtikelPositionen { get; set; } = new List<WarenausgangArtikelPositionen>();
}