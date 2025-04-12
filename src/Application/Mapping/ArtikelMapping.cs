using System;
using Artikelsystem.Shared.DTOs.Artikel.Response;
using Domain.Entities.Artikel;
using Domain.Entities.Wareneingang;
using Domain.Entities.Warenausgang;

using Profile = AutoMapper.Profile;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;
using Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Response;
namespace Application.Mapping;

public class ArtikelMapping : Profile
{
    public ArtikelMapping()
    {
        // DTO → Entity
        CreateMap<ArtikelDto, Artikel>()
            .ForMember(dest => dest.ArtikelStatistik, opt => opt.Ignore())
            .ForMember(dest => dest.Wareneingaenge, opt => opt.Ignore())
            .ForMember(dest => dest.Warenausgaenge, opt => opt.Ignore());

        // Entity → DTO
        CreateMap<Artikel, ArtikelDto>()
            .ForMember(dest => dest.ArtikelStatistik, opt => opt.MapFrom(src => src.ArtikelStatistik))
            .ForMember(dest => dest.WareneingangArtikelPositionen, opt => opt.MapFrom(src => 
                src.Wareneingaenge != null ? src.Wareneingaenge : null))
            .ForMember(dest => dest.WarenausgangArtikelPositionen, opt => opt.MapFrom(src => 
                src.Warenausgaenge != null ? src.Warenausgaenge : null));
        
        // ArtikelStatistik mappings
        CreateMap<ArtikelStatistik, ArtikelStatistikDto>();
        CreateMap<ArtikelStatistikDto, ArtikelStatistik>();
        
        // WareneingangArtikelPositionen mappings
        CreateMap<Domain.Entities.Wareneingang.WareneingangArtikelPositionen, WareneingangArtikelPositionenDto>()
            .ForMember(dest => dest.Wareneingang, opt => opt.MapFrom(src => src.Wareneingang))
            // Vermeidung von Zirkelreferenzen beim Mapping
            .ForMember(dest => dest.ArtikelId, opt => opt.Ignore());
        
        CreateMap<WareneingangArtikelPositionenDto, WareneingangArtikelPositionen>()
            .ForMember(dest => dest.Wareneingang, opt => opt.MapFrom(src => src.Wareneingang))
            .ForMember(dest => dest.Artikel, opt => opt.Ignore());
        
        // WarenausgangArtikelPositionen mappings
        CreateMap<WarenausgangArtikelPositionen, WarenausgangArtikelPositionenDto>()
            .ForMember(dest => dest.Warenausgang, opt => opt.MapFrom(src => src.Warenausgang))
            // Vermeidung von Zirkelreferenzen beim Mapping
            .ForMember(dest => dest.Artikel, opt => opt.Ignore())
            .ForMember(dest => dest.ArtikelName, opt => opt.MapFrom(src => src.Artikel != null ? src.Artikel.Name : ""));
        
        CreateMap<WarenausgangArtikelPositionenDto, Domain.Entities.Warenausgang.WarenausgangArtikelPositionen>()
            .ForMember(dest => dest.Warenausgang, opt => opt.MapFrom(src => src.Warenausgang))
            .ForMember(dest => dest.Artikel, opt => opt.Ignore());
        
        // Wareneingang mappings
        CreateMap<Domain.Entities.Wareneingang.Wareneingaenge, WarenausgangDto>()
            // Vermeidung von Zirkelreferenzen beim Mapping
            .ForMember(dest => dest.ArtikelPositionen, opt => opt.Ignore()); 
        
        // Warenausgang mappings
        CreateMap<Domain.Entities.Warenausgang.Warenausgaenge, WarenausgangDto>()
            // Vermeidung von Zirkelreferenzen beim Mapping
            .ForMember(dest => dest.ArtikelPositionen, opt => opt.Ignore());
    }
}
