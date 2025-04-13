using System;

namespace Artikelsystem.Shared.DTOs;

public class PagedResultDTO<T> where T : class
{
    public List<T> Items { get; set; } = new List<T>();
    public int Page { get; set; }
    public int RecordsPerPage { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / RecordsPerPage);
}
