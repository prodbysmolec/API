using System;

namespace Artikelsystem.Domain.Entities.Artikel;

public class Zusatzfeld
{
    public int ZusatzfeldID { get; set; }
    public required string Name { get; set; }
    public virtual ICollection<Zusatzwert> ZusatzWerte { get; set; } = new HashSet<Zusatzwert>();
    public virtual ICollection<ArtikelgruppeZusatzfelder> ArtikelGruppeZusatzFelder { get; set; } = new HashSet<ArtikelgruppeZusatzfelder>();
    public bool IsChecked { get; set; } = false;
}
