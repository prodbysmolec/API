using System;

namespace Artikelsystem.Api.Features.Artikel.Models.Entitys;

public class ArtikelZusatzWert
{
    public int ArtikelId { get; set; }
    public int ZusatzwertId { get; set; }
    public virtual Artikel Artikel { get; set; } = null!;
    public virtual Zusatzwert Zusatzwert { get; set; } = null!;
}
