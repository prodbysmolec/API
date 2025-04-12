using System;

namespace Artikelsystem.Shared.DTOs.Lieferant;

public class LieferantDetailDto : LieferantDto
{
    public int ArtikelAnzahl { get; set; }
    public List<LieferantArtikelDto> AktiveArtikel { get; set; } = new List<LieferantArtikelDto>();
}
