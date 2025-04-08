using System;

namespace Artikelsystem.Api.Features.Artikel.Models.Entitys;

public class Zusatzwert
{
    public int Id { get; set; }
    public required string Wert { get; set; }
    public int ZusatzFeldID { get; set; }
    public Zusatzfeld ZusatzFeld { get; set; } = null!;
    public bool IsChecked { get; set; }
    public virtual ICollection<ArtikelZusatzWert> ArtikelZusatzwerte { get; set; } = new List<ArtikelZusatzWert>();
}