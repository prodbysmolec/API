using System;
using FluentValidation;

namespace Artikelsystem.Shared.DTOs;

public class PagedResultDTO<T> where T : class
{
    public List<T> Items { get; set; } = new List<T>();
    public int Page { get; set; } = 1;
    public int RecordsPerPage { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / RecordsPerPage);
}

public class PagedResultDTOValidator : AbstractValidator<PagedResultDTO<object>>
{
    public PagedResultDTOValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page muss größer als 0 sein.");

        RuleFor(x => x.RecordsPerPage)
            .GreaterThan(0)
            .WithMessage("Die angezeigten Datensätze pro Seite müssen größer als 0 sein.");
    }
}
