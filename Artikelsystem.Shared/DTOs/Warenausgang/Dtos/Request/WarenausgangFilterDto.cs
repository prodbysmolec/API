using System;

namespace Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Filter;

public class WarenausgangFilterDto
{
    public DateTime? VonDatum { get; set; }
    public DateTime? BisDatum { get; set; }
    public string? ErstelltVon { get; set; }
    public string? GeaendertVon { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? Suchbegriff { get; set; }
    public bool? SortDescending { get; set; } = false;
    public string? SortBy { get; set; } = "Id";
}
