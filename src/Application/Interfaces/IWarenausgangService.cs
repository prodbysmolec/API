using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Filter;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Request;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;

namespace Application.Interfaces;

public interface IWarenausgangService
{
    Task<PagedResultDTO<WarenausgangDto>> GetWarenausgaengeAsync(WarenausgangFilterDto filter, int pageNumber, int pageSize);
    Task<WarenausgangDto?> GetWarenausgangByIdAsync(int id);
    Task<WarenausgangDto> CreateWarenausgangAsync(WarenausgangRequestDto dto);
    Task<List<WarenausgangArtikelPositionenDto>> GetWarenausgangArtikelPositionenByWarenausgangIdAsync(int warenausgangId);
    Task<bool> DeleteWarenausgangAsync(int id);
}