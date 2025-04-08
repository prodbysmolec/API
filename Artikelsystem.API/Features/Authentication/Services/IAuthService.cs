using System;
using Artikelsystem.API.Features.Authentication.Models.Entitys;
using Artikelsystem.Shared.DTOs.User.Request;

namespace Artikelsystem.API.Features.Authentication.Services;

public interface IAuthService
{
    Task<User?> RegisterAsync(UserDto request);
    Task<TokenResponseDto?> LoginAsync(UserLoginDto request);
    Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
}
