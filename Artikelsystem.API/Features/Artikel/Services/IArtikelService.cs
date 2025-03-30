using Artikelsystem.Api.Features.Artikel.Models.DTOs;
using Artikelsystem.Api.Features.Employees.Enums;

public interface IArtikelService
{
    Task<IEnumerable<ArtikelDto>> GetAllArtikelAsync();
    Task<ArtikelDto?> GetArtikelByIdAsync(int id);
    Task<ArtikelDto> CreateArtikelAsync(CreateArtikelRequest request);
    Task<ArtikelDto?> UpdateArtikelAsync(int id, UpdateArtikelRequest request);
    Task<bool> DeleteArtikelAsync(int id);
    Task<IEnumerable<ArtikelDto>> GetArtikelMitBestandUnterMindestbestandAsync();
    Task<bool> UpdateArtikelBestandAsync(int artikelId, int menge);
    Task<bool> UpdateArtikelStatusAsync(int artikelId, ArtikelStatus status);
    Task<bool> AktualisiereArtikelStatistikAsync(int artikelId);
}