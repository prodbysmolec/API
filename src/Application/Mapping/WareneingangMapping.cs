using Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Response;
using AutoMapper;
using Domain.Entities.Wareneingang;

namespace Application.Mapping;

public class WareneingangMapping : Profile
{
    public WareneingangMapping()
    {
        CreateMap<Wareneingaenge, GetWareneingaengeForArtikelResponse>()
            .ForMember(dest => dest.ArtikelPositionen, opt => opt.MapFrom(src => src.WareneingangsPositionen));

        CreateMap<WareneingangArtikelPositionen, WareneingangArtikelPositionenDto>()
            .ForMember(dest => dest.ArtikelName, opt => opt.MapFrom(src => src.Artikel!.Name));
    }
}
