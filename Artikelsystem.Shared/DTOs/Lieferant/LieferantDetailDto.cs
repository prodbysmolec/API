using System;

namespace Artikelsystem.Api.Features.Lieferant.Models.DTOs;

public class LieferantDetailDto : LieferantDto
{
    public int ArtikelAnzahl { get; set; }
    public List<LieferantArtikelDto> AktiveArtikel { get; set; } = new List<LieferantArtikelDto>();
}
