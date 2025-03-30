using System;
using Artikelsystem.Api.Features.Lieferant.Models.DTOs;

namespace Artikelsystem.Api.Features.Lieferant.Services;

public interface ILieferantService
{
    Task<IEnumerable<LieferantDto>> GetAllLieferantenAsync(int page = 1, int recordsPerPage = 100, string? firmaContains = null, string? nameContains = null);
    Task<LieferantDto?> GetLieferantByIdAsync(int id);
    Task<LieferantDto> CreateLieferantAsync(CreateLieferantRequest request);
    Task<LieferantDto?> UpdateLieferantAsync(int id, UpdateLieferantRequest request);
    Task<bool> DeleteLieferantAsync(int id);
}