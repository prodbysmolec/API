namespace Artikelsystem.Api.Features.Warenausgang.Models.DTOs.Requests;

public class CreateWarenausgangRequest
{
    public string? AllgemeineBemerkungen { get; set; }

    public required List<CreateWarenausgangArtikelPositionRequest> ArtikelPositionen { get; set; }
}
