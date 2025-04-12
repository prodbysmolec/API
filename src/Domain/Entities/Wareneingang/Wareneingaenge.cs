using System;
using Domain.Entities.Lieferant;
using Domain.Common;

namespace Domain.Entities.Wareneingang;

public class Wareneingaenge : AuditableEntity
{
    public int Id { get; set; }
    //public int LieferantId { get; set; }
    public required decimal Gesamtpreis { get; set; }
    public string? AllgemeineBemerkungen { get; set; } = "";
    // Navigation properties
    public virtual ICollection<WareneingangArtikelPositionen> WareneingangsPositionen { get; set; } = new List<WareneingangArtikelPositionen>();
}
