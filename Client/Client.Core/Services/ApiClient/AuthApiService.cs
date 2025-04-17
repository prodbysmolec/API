using System;
using Artikelsystem.Shared.Constants;
using Client.Core.Services.Auth;

namespace Client.Core.Services.ApiClient;

public class AuthApiService
{
    private readonly HttpClientBase _httpClient;
    private readonly ITokenService _tokenService;

    public AuthApiService(HttpClientBase httpClient, ITokenService tokenService)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        return await _tokenService.LoginAsync(username, password);
    }

    public async Task<bool> RegisterAsync(string username, string password, string email)
    {
        return await _tokenService.RegisterAsync(username, password, email);
    }

    public async Task LogoutAsync()
    {
        await _tokenService.LogoutAsync();
    }
    
    public async Task<bool> IsAuthenticatedAsync()
    {
        return await _tokenService.IsAuthenticatedAsync();
    }
    
    public async Task<bool> IsAdminAsync()
    {
        return await _tokenService.IsAdminAsync();
    }
    
    public async Task<string> CheckAdminEndpointAsync()
    {
        try
        {
            return await _httpClient.GetAsync<string>(ApiRoutes.Authentication.AdminOnly);
        }
        catch (ApiException ex)
        {
            if (ex.IsUnauthorized)
            {
                return "You are not logged in";
            }
            if (ex.IsForbidden)
            {
                return "You are not an admin";
            }
            return $"Error: {ex.Message}";
        }
    }
    
    public async Task<string> CheckAuthenticatedEndpointAsync()
    {
        try
        {
            return await _httpClient.GetAsync<string>(ApiRoutes.Authentication.AuthenticateOnly);
        }
        catch (ApiException ex)
        {
            if (ex.IsUnauthorized)
            {
                return "You are not logged in";
            }
            return $"Error: {ex.Message}";
        }
    }
}

