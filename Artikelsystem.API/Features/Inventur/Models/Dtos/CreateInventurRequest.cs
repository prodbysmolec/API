using System;

namespace Artikelsystem.Api.Features.Inventur.Models.Dtos;

public class CreateInventurRequest
{
    public string Bezeichnung { get; set; } = string.Empty;
    public string? Bemerkung { get; set; }
    public string ErstelltVon { get; set; } = string.Empty;
}