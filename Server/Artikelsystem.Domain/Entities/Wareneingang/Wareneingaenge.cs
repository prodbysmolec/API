using System;
using Artikelsystem.Api.Features.Lieferant.Models.Entitys;
using Artikelsystem.Domain.Common;

namespace Artikelsystem.Api.Features.Wareneingang.Models.Entitys;

public class Wareneingaenge : AuditableEntity
{
    public int Id { get; set; }
    //public int LieferantId { get; set; }
    public required decimal Gesamtpreis { get; set; }
    public string? AllgemeineBemerkungen { get; set; } = "";
    // Navigation properties
    public virtual ICollection<WareneingangArtikelPositionen> WareneingangsPositionen { get; set; } = new List<WareneingangArtikelPositionen>();
}
