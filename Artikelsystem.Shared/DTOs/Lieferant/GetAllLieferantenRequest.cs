using System;

namespace Artikelsystem.Shared.DTOs.Lieferant.Request;

public class GetAllLieferantenRequest
{
    public bool? nurAktive { get; set; } = false;
    public bool? alle { get; set; } = false;
}
