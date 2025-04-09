namespace Artikelsystem.Api.Features.Lieferant.Models.DTOs;

public class UpdateLieferantRequest : CreateLieferantRequest
{
    public bool IstAktiv { get; set; }
}