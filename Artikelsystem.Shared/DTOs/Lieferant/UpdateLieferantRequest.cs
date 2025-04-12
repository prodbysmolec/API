namespace Artikelsystem.Shared.DTOs.Lieferant;

public class UpdateLieferantRequest : CreateLieferantRequest
{
    public bool IstAktiv { get; set; }
}