using System;

namespace Client.Core.Services.Auth;

public interface ITokenService
{
    Task<string> GetAccessTokenAsync();
    Task<bool> LoginAsync(string username, string password);
    Task<bool> RegisterAsync(string username, string password, string email);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<bool> IsAdminAsync();
}
