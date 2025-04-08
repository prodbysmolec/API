using System;
using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Filter;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;

namespace Artikelsystem.API.Features.Warenausgang.Service;

public interface IWarenausgangService
{
    // wie kann ich das hier mit PagedResultDTO machen?
    Task<PagedResultDTO<WarenausgangDto>> GetWarenausgaengeAsync(WarenausgangFilterDto filter, int pageNumber, int pageSize);

    // GetWarenausgangByIdAsync
    Task<WarenausgangDto?> GetWarenausgangByIdAsync(int id);
}
