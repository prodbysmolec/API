using System;
using Artikelsystem.Shared.DTOs.Warenausgang.Enums;

namespace Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;

public class WarenausgangDto
{
    public int Id { get; set; }
    public string? AllgemeineBemerkungen { get; set; }
    public required WarenausgangZweckEnum Zweck { get; set; } = 0;
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string? CreatedBy { get; set; } = "";
    public string? UpdatedBy { get; set; } = "";
    public virtual List<WarenausgangArtikelPositionenDto> ArtikelPositionen { get; set; } = new List<WarenausgangArtikelPositionenDto>();
}
