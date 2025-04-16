using Domain.Entities.Authentication;
using Artikelsystem.Shared.DTOs.User.Request;
using Domain.Common.ResultPattern;
using Application.Commands.Authentication;

namespace Application.Interfaces;

public interface IAuthenticationService
{
    Task<Result<User>?> RegisterAsync(UserDto request);
    Task<Result<TokenResponseDto>?> LoginAsync(LoginCommand request);
    Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
}
