using System;

namespace Artikelsystem.Api.Features.Artikel.Models.Entitys;

public class Produktkategorie
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Beschreibung { get; set; }
    public virtual ICollection<Artikelgruppe> ArtikelGruppen { get; set; } = new HashSet<Artikelgruppe>();
}
