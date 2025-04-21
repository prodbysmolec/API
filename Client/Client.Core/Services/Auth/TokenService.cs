using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Artikelsystem.Shared.Constants;
using Artikelsystem.Shared.DTOs.User.Request;

namespace Client.Core.Services.Auth
{
    public class TokenService : ITokenService
    {
        private readonly HttpClient _httpClient;
        private readonly ISecureStorage _secureStorage;
        private readonly string _baseUrl;

        private const string ACCESS_TOKEN_KEY = "access_token";
        private const string REFRESH_TOKEN_KEY = "refresh_token";
        private const string TOKEN_EXPIRY_KEY = "token_expiry";

        public TokenService(string baseUrl, ISecureStorage secureStorage)
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            _secureStorage = secureStorage;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var token = await _secureStorage.GetAsync(ACCESS_TOKEN_KEY);
            var expiryString = await _secureStorage.GetAsync(TOKEN_EXPIRY_KEY);

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(expiryString))
            {
                return string.Empty;
            }

            if (DateTime.TryParse(expiryString, out var expiry) && expiry <= DateTime.UtcNow.AddMinutes(5))
            {
                // Token is expired or about to expire, try to refresh
                return await RefreshTokenAsync();
            }

            return token;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var loginDto = new UserLoginDto
                {
                    Username = username,
                    Password = password
                };
                string loginEndpoint = $"{_baseUrl.TrimEnd('/')}/{ApiRoutes.Authentication.Login.TrimStart('/')}";

                var response = await _httpClient.PostAsJsonAsync(
                    ApiRoutes.Authentication.Login,
                    loginDto);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponseDto>();
                if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    return false;
                }

                var responseString = await response.Content.ReadAsStringAsync();

                await StoreTokensAsync(
                    tokenResponse.AccessToken,
                    tokenResponse.RefreshToken,
                    tokenResponse.ExpiresAt);

                return true;
            }
            catch (Exception ex)
            {
                var xyz = ex.Message;
                throw new Exception("Login failed", ex);
                return false;
            }
        }

        public async Task<bool> RegisterAsync(string username, string password, string email)
        {
            try
            {
                var registerCommand = new RegisterCommand
                {
                    Username = username,
                    Password = password,
                    Email = email
                };

                var response = await _httpClient.PostAsJsonAsync(
                    ApiRoutes.Authentication.Register,
                    registerCommand);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task LogoutAsync()
        {
            await _secureStorage.RemoveAsync(ACCESS_TOKEN_KEY);
            await _secureStorage.RemoveAsync(REFRESH_TOKEN_KEY);
            await _secureStorage.RemoveAsync(TOKEN_EXPIRY_KEY);
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetAccessTokenAsync();
            return !string.IsNullOrEmpty(token);
        }

        public async Task<bool> IsAdminAsync()
        {
            var token = await GetAccessTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var roleClaims = jwtToken.Claims
                    .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                    .Select(c => c.Value);

                return roleClaims.Any(r => r.Equals("Admin", StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                return false;
            }
        }

        private async Task<string> RefreshTokenAsync()
        {
            var refreshToken = await _secureStorage.GetAsync(REFRESH_TOKEN_KEY);

            if (string.IsNullOrEmpty(refreshToken))
            {
                return string.Empty;
            }

            try
            {
                var refreshRequest = new RefreshTokenRequestDto
                {
                    RefreshToken = refreshToken
                };

                var response = await _httpClient.PostAsJsonAsync(
                    "/authentication/refresh-token", // Adjust this endpoint as needed
                    refreshRequest);

                if (!response.IsSuccessStatusCode)
                {
                    await LogoutAsync();
                    return string.Empty;
                }

                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponseDto>();
                if (tokenResponse == null)
                {
                    await LogoutAsync();
                    return string.Empty;
                }

                await StoreTokensAsync(
                    tokenResponse.AccessToken,
                    tokenResponse.RefreshToken,
                    tokenResponse.ExpiresAt);

                return tokenResponse.AccessToken;
            }
            catch
            {
                await LogoutAsync();
                return string.Empty;

            }
        }

        private async Task StoreTokensAsync(string accessToken, string refreshToken, DateTime expiresAt)
        {
            await _secureStorage.SetAsync(ACCESS_TOKEN_KEY, accessToken);
            await _secureStorage.SetAsync(REFRESH_TOKEN_KEY, refreshToken);
            await _secureStorage.SetAsync(TOKEN_EXPIRY_KEY, expiresAt.ToString("o"));
        }
    }

    // Define the token response class if not already defined in your shared project
    public class TokenResponseDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    // Define the register command if not already defined
    public class RegisterCommand
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
    }
}