using Artikelsystem.Api.Features.Lieferant.Models.DTOs;
using Artikelsystem.API.Features.Lieferant.Models.DTOs.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artikelsystem.Api.Features.Lieferant.Services
{
    public interface ILieferantService
    {
        Task<List<LieferantDto>> GetAllLieferanten(bool nurAktiv = false, bool alles = false);
        Task<LieferantDetailDto?> GetLieferantById(bool? alles, int id);
        Task<LieferantDto> ErstelleLieferant(CreateLieferantRequest request);
        Task<LieferantDto?> UpdateLieferantAsync(int id, UpdateLieferantRequest request);
        Task<bool> DeactivateLieferantAsync(int id);
        Task<bool> DeleteLieferantAsync(int id);
        Task<List<LieferantDto>> SearchLieferantenAsync(string suchbegriff);
    }
}
