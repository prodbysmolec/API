using System;

namespace Artikelsystem.Domain.Entities.Artikel;

public class ArtikelZusatzWert
{
    public int ArtikelId { get; set; }
    public int ZusatzwertId { get; set; }
    public virtual Artikel Artikel { get; set; } = null!;
    public virtual Zusatzwert Zusatzwert { get; set; } = null!;
}
