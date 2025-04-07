using System;

namespace Artikelsystem.API.Features.Lieferant.Models.DTOs.Request;

public class GetAllLieferantenRequest
{
    public bool? nurAktive { get; set; } = false;
    public bool? alle { get; set; } = false;
}
