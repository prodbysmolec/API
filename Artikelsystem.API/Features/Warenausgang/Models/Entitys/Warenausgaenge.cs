using System;
using System.ComponentModel.DataAnnotations.Schema;
using Artikelsystem.Api.Models;
using Artikelsystem.Shared.DTOs.Warenausgang.Enums;

namespace Artikelsystem.Api.Features.Warenausgang.Models.Entitys;

public class Warenausgaenge : AuditableEntity
{
    public int Id { get; set; }
    public string? AllgemeineBemerkungen { get; set; }
    [Column("Zweck")]
    public required WarenausgangZweckEnum Zweck { get; set; } = 0;

    // Navigation Property
    public virtual ICollection<WarenausgangArtikelPositionen> ArtikelPositionen { get; set; } = new List<WarenausgangArtikelPositionen>();
}