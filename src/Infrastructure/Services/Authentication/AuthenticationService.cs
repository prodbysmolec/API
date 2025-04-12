using System;
using Application.Interfaces;
using Domain.Entities.Authentication;
using Artikelsystem.Shared.DTOs.User.Request;

namespace Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{


    //private readonly IUserRepository _userRepository; // Falls du ein Repository hast, um User zu verwalten.

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
