using System;

namespace Artikelsystem.Shared.Helfer;

public class FilteringDTO
{
    public int Page { get; set; } = 1; // Standardwert: Seite 1
    public int RecordsPerPage { get; set; } = 10; // Standardwert: 10 Einträge pro Seite
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; }
    public string? NameContains { get; set; } // Optionaler Filter
}
