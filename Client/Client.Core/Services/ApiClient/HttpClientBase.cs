using System;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Artikelsystem.Shared;
using Client.Core.Services.Auth;

namespace Client.Core.Services.ApiClient;

public class HttpClientBase : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;
    private readonly JsonSerializerOptions _jsonOptions;
    private bool _disposedValue;

    public HttpClientBase(string baseUrl, ITokenService tokenService)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };

        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        
        _tokenService = tokenService;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    /// <summary>
    /// Sendet ein GET-Request an den angegebenen Endpunkt und gibt die Antwort als deserialisiertes Objekt zurück.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="endpoint"></param>
    /// <returns></returns>
    public async Task<T> GetAsync<T>(string endpoint)
    {
        await AddAuthHeaderAsync();

        var response = await _httpClient.GetAsync(endpoint);
        return await HandleResponseAsync<T>(response);
    }

    /// <summary>
    /// Sendet ein POST-Request an den angegebenen Endpunkt mit den angegebenen Daten und gibt die Antwort als deserialisiertes Objekt zurück.
    /// </summary>
    public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        await AddAuthHeaderAsync();

        var content = new StringContent(
            JsonSerializer.Serialize(data, _jsonOptions),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync(endpoint, content);
        return await HandleResponseAsync<TResponse>(response);
    }

    /// <summary>
    /// Sendet ein PUT-Request an den angegebenen Endpunkt mit den angegebenen Daten und gibt die Antwort als deserialisiertes Objekt zurück.
    /// </summary>
    public async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        await AddAuthHeaderAsync();

        var content = new StringContent(
            JsonSerializer.Serialize(data, _jsonOptions),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PutAsync(endpoint, content);
        return await HandleResponseAsync<TResponse>(response);
    }

    /// <summary>
    /// Sendet ein DELETE-Request an den angegebenen Endpunkt und gibt die Antwort als deserialisiertes Objekt zurück.
    /// </summary>
    public async Task<T> DeleteAsync<T>(string endpoint)
    {
        await AddAuthHeaderAsync();

        var response = await _httpClient.DeleteAsync(endpoint);
        return await HandleResponseAsync<T>(response);
    }

    private async Task<T> HandleResponseAsync<T>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            try
            {
                if (typeof(T) == typeof(object) || typeof(T) == typeof(void))
                {
                    // For void returns or when we don't care about the response body
                    return default!; // Null-forgiving operator tells compiler we know what we're doing
                }
                
                var result = await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
                if (result == null)
                {
                    throw new ApiException("Response content was null", HttpStatusCode.UnprocessableEntity);
                }
                return result;
            }
            catch (JsonException ex)
            {
                throw new ApiException("Failed to parse response", HttpStatusCode.UnprocessableEntity, ex);
            }
        }
        
        // Handle error responses
        var errorContent = await response.Content.ReadAsStringAsync();
        
        try
        {
            // Try to deserialize as ApiResponseDTO (your error format)
            var errorResponse = JsonSerializer.Deserialize<ApiResponseDTO>(errorContent, _jsonOptions);
            throw new ApiException(
                errorResponse?.Message ?? "An error occurred", 
                response.StatusCode);
        }
        catch (JsonException)
        {
            // If can't parse as ApiResponseDTO, use raw error content
            throw new ApiException(
                !string.IsNullOrEmpty(errorContent) ? errorContent : $"HTTP error {(int)response.StatusCode}",
                response.StatusCode);
        }
    }

    /// <summary>
    /// Fügt den Authorization-Header mit dem Token hinzu, wenn verfügbar.
    /// </summary>
    private async Task AddAuthHeaderAsync()
    {
        if (_tokenService != null)
        {
            var token = await _tokenService.GetAccessTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
            }
        }
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _httpClient.Dispose();
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
