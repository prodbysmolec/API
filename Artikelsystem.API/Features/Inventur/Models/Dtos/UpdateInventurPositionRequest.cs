using System;

namespace Artikelsystem.Api.Features.Inventur.Models.Dtos;

public class UpdateInventurPositionRequest
{
    public int PositionId { get; set; }
    public int ArtikelId { get; set; }
    public int InventurID { get; set; }
    public int GezaehlteMenge { get; set; }
    public string? Bemerkung { get; set; }
    public string BearbeitetVon { get; set; } = string.Empty;
    public bool IstGeprueft { get; set; } = false;
}