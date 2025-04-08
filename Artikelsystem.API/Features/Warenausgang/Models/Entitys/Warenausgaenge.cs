using System;
using Artikelsystem.Api.Models;
using Artikelsystem.Shared.DTOs.Warenausgang.Enums;

namespace Artikelsystem.Api.Features.Warenausgang.Models.Entitys;

public class Warenausgaenge : AuditableEntity
{
    public int Id { get; set; }

    // Allgemeine Bemerkungen zum Warenausgang
    public string? AllgemeineBemerkungen { get; set; }
    public required WarenausgangZweckEnum Zweck { get; set; } = 0;

    // Navigation Property
    public virtual ICollection<WarenausgangArtikelPositionen> ArtikelPositionen { get; set; } = new List<WarenausgangArtikelPositionen>();
}