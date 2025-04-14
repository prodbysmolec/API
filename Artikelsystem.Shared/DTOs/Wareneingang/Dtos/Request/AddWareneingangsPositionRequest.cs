using System;
using System.ComponentModel.DataAnnotations;

namespace Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Request;

public class AddWareneingangsPositionRequest
{
    [Required]
    public int WareneingangId { get; set; }

    [Required]
    public int ArtikelId { get; set; }

    [Required]
    public int Menge { get; set; }

    [Required]
    public decimal Einzelpreis { get; set; }
}
