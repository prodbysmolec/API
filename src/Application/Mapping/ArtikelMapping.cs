using System;
using Artikelsystem.Shared.DTOs.Artikel.Response;
using Domain.Entities.Artikel;
using Domain.Entities.Wareneingang;
using Domain.Entities.Warenausgang;

using Profile = AutoMapper.Profile;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;
using Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Response;
using Artikelsystem.Shared.DTOs.Artikel.Request;
using Application.Commands.Artikel;
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

        CreateMap<CreateArtikelRequest, Artikel>()
                .ForMember(dest => dest.Bild, opt => opt.Ignore()) // ggf. extra behandeln
                .ForMember(dest => dest.ArtikelGruppeId, opt => opt.MapFrom(src => src.ArtikelGruppeId));

        CreateMap<Artikel, CreateArtikelCommand>();

        CreateMap<CreateArtikelCommand,Artikel>();

        CreateMap<GetAllArtikelRequest, Artikel>();
        CreateMap<Artikel, GetAllArtikelRequest>();


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
                
        CreateMap<Artikel, GetArtikelResponse>()
            .ForMember(dest => dest.BildBase64, opt => opt.MapFrom(src => src.Bild != null ? Convert.ToBase64String(src.Bild) : null))
            .ForMember(dest => dest.Statistik, opt => opt.MapFrom(src => src.ArtikelStatistik != null ? new ArtikelStatistikDto
            {
                DurchschnittlicherEinzelpreis = src.ArtikelStatistik.DurchschnittlicherEinzelpreis,
                Lagerwert = src.ArtikelStatistik.Lagerwert
            } : null));
    }
}
