using System;

namespace Artikelsystem.Api.Features.Wareneingang.Models.DTOs.Requests;

public class CreateWareneingangRequest
{
    public required decimal Gesamtpreis { get; set; }
    public string? AllgemeineBemerkungen { get; set; }
}
