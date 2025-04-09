using System;

namespace Artikelsystem.Domain.Entities.Artikel;


public class Artikelgruppe
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int ProduktkategorieId { get; set; }
    public virtual Produktkategorie? Produktkategorie { get; set; }
    public virtual ICollection<ArtikelgruppeZusatzfelder> ArtikelgruppeZusatzfelder { get; set; } = new HashSet<ArtikelgruppeZusatzfelder>();
}
