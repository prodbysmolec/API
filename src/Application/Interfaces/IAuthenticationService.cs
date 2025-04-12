using Domain.Entities.Authentication;
using Artikelsystem.Shared.DTOs.User.Request;

namespace Application.Interfaces;

public interface IAuthenticationService
{
    Task<User?> RegisterAsync(UserDto request);
    Task<TokenResponseDto?> LoginAsync(UserLoginDto request);
    Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
}
