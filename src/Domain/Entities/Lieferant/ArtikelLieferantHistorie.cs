using System;

namespace Domain.Entities.Lieferant;

public class ArtikelLieferantHistorie
{
    public int Id { get; set; }
    public int ArtikelId { get; set; }
    public int LieferantId { get; set; }
    public decimal Einkaufspreis { get; set; }
    public DateTime? GueltigVon { get; set; }
    public DateTime? GueltigBis { get; set; }
    public DateTime Erstellungsdatum { get; set; } = DateTime.UtcNow;
}
