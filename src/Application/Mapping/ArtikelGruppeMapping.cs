using System;
using Artikelsystem.Shared.DTOs.ArtikelGruppe.Request;
using AutoMapper;
using Domain.Entities.Artikel;

namespace Application.Mapping;

public class ArtikelGruppeMapping : Profile
{
    public ArtikelGruppeMapping()
    {
        // Mapping von ArtikelGruppe zu GetAllArtikelGruppeResponse
        CreateMap<Artikelgruppe, GetAllArtikelGruppeResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ProduktKategorieId, opt => opt.MapFrom(src => src.ProduktkategorieId));
    }

}
