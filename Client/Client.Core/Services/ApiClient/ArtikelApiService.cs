using System;
using Artikelsystem.Shared;
using Artikelsystem.Shared.Constants;
using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.Artikel.Request;
using Artikelsystem.Shared.DTOs.Artikel.Response;

namespace Client.Core.Services.ApiClient;

public class ArtikelApiService
{
    private readonly HttpClientBase _httpClient;

    public ArtikelApiService(HttpClientBase httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PagedResultDTO<ArtikelDto>> GetAllArtikelAsync(GetAllArtikelRequest request)
    {
        return await _httpClient.PostAsync<GetAllArtikelRequest, PagedResultDTO<ArtikelDto>>(
            ApiRoutes.Artikel.GetAllArtikel, request);
    }
    
    public async Task<ArtikelDto> GetArtikelByIdAsync(int id)
    {
        var endpoint = ApiRoutes.Artikel.GetArtikelById.Replace("{id}", id.ToString());
        return await _httpClient.GetAsync<ArtikelDto>(endpoint);
    }
    
    public async Task<ApiResponseDTO> CreateArtikelAsync(CreateArtikelRequest request)
    {
        return await _httpClient.PostAsync<CreateArtikelRequest, ApiResponseDTO>(
            ApiRoutes.Artikel.CreateArtikel, request);
    }
}
