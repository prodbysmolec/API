using System;
using Domain.Entities.Authentication;
using Artikelsystem.Shared.DTOs.User.Request;

namespace API.Features.Authentication.Services;

public interface IAuthService
{
    Task<User?> RegisterAsync(UserDto request);
    Task<TokenResponseDto?> LoginAsync(UserLoginDto request);
    Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
}
