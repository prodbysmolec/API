using System;
using Application.Interfaces;
using Domain.Entities.Authentication;
using Artikelsystem.Shared.DTOs.User.Request;

namespace Application.Services;

public class AuthenticationService : IAuthenticationService
{
    public Task<TokenResponseDto?> LoginAsync(UserLoginDto request)
    {
        throw new NotImplementedException();
    }

    public Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
    {
        throw new NotImplementedException();
    }

    public Task<User?> RegisterAsync(UserDto request)
    {
        throw new NotImplementedException();
    }
}
