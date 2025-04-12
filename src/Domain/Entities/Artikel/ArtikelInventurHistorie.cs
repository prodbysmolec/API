using System;

namespace Domain.Entities.Artikel;

public class ArtikelInventurHistorie
{
    public int Id { get; set; }
    public int ArtikelId { get; set; }
    public int InventurId { get; set; }
    public int AlteBestandsmenge { get; set; }
    public int NeueBestandsmenge { get; set; }
    public int Differenz { get; set; }
    public decimal DifferenzWert { get; set; }
    public DateTime Datum { get; set; }

    // Navigation Propertys
    public virtual Artikel Artikel { get; set; } = null!;
    public virtual Inventur.Inventur Inventur { get; set; } = null!;
}
