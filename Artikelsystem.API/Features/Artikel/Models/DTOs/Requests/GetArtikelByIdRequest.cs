using System;

namespace Artikelsystem.Api.Features.Artikel.Models.DTOs;

public class GetArtikelByIdRequest
{
    public bool IncludeArtikelStatistik { get; set; } = false;
    public bool IncludeWareneingaenge { get; set; } = false;
    public bool IncludeWarenausgaenge { get; set; } = false;

    public bool IncludeLieferanten { get; set;} = false;
    //public bool IncludeArtikelGruppen { get; set; } = false;
    //public bool IncludeArtikelZusatzwerte { get; set;} = false;

}
