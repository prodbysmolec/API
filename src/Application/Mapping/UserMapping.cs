using System;
using Artikelsystem.Shared.DTOs.User.Request;
using AutoMapper;
using Domain.Entities.Authentication;
using Microsoft.CodeAnalysis;

namespace Application.Mapping;

public class UserMapping : Profile
{
    public UserMapping()
    {
        // User -> UserDto
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Nachname, opt => opt.MapFrom(src => src.Nachname))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.Ignore()); // PasswordHash wird separat verarbeitet

        // Mapping von UserDto zu User (neu hinzugefügt)
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Nachname, opt => opt.MapFrom(src => src.Nachname))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // PasswordHash wird separat verarbeitet
    }
}
