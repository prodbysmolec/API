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
        var query = $"?PageNumber={request.Page}&PageSize={request.RecordsPerPage}";
        var url = $"{ApiRoutes.Artikel.GetAllArtikel}{query}";

        return await _httpClient.GetAsync<PagedResultDTO<ArtikelDto>>(url);
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
