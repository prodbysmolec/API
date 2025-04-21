using System;
using Application.Interfaces;
using Domain.Entities.Authentication;
using Artikelsystem.Shared.DTOs.User.Request;
using Microsoft.AspNetCore.Identity;
using Application.Authentication;
using AutoMapper;
using Domain.Common.ResultPattern;
using Microsoft.Extensions.Logging;
using Domain.Common.BaseErrors;
using Application.Commands.Authentication;
using Domain.Errors;
using Application.Interfaces.Repositories;

namespace Application.Services;

public class AuthenticationService(
    IUserRepository UserRepository,
    IJwtTokenGenerator tokenService,
    IPasswordService passwordService,
    IMapper mapper,
    ILogger<AuthenticationService> logger
) : IAuthenticationService
{
    private readonly ILogger<AuthenticationService> _logger = logger;
    private readonly IUserRepository _UserRepository = UserRepository;
    private readonly IJwtTokenGenerator _tokenService = tokenService;
    private readonly IPasswordService _passwordService = passwordService;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<TokenResponseDto>?> LoginAsync(LoginCommand request)
    {
        _logger.LogInformation("Login für Benutzer {Username} gestartet", request.Username);
        try
        {
            var user = await _UserRepository.GetByUserNameAsync(request.Username);

            if (user == null || user.Value == null)
            {
                _logger.LogWarning("Login fehlgeschlagen: Benutzername {Username} existiert nicht", request.Username);
                return Result<TokenResponseDto>.Failure(UserError.InvalideCredentials());
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user.Value, user.Value.PasswordHash!, request.Password)
                == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning("Login fehlgeschlagen: Falsches Passwort für Benutzername {Username}", request.Username);
                return Result<TokenResponseDto>.Failure(UserError.InvalideCredentials());
            }

            _logger.LogInformation("Login erfolgreich für Benutzername {Username}", request.Username);
            return await CreateTokenResponse(user.Value);
        }
        catch
        {
            _logger.LogError("Fehler beim Login für Benutzer {Username}", request.Username);
            return Result<TokenResponseDto>.Failure(BaseError.InternalServerError("LoginFehlgeschlagen", "Fehler beim Login."));
        }

    }

    private async Task<TokenResponseDto> CreateTokenResponse(User user)
    {
        return new TokenResponseDto
        {
            AccessToken = await _tokenService.CreateAccessTokenAsync(user),
            RefreshToken = await _tokenService.GenerateAndSaveRefreshTokenAsync(user)
        };
    }

    public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
    {
        var user = await _UserRepository.ValidateRefreshTokenAsync(request.UserID, request.RefreshToken);
        if (user == null)
        {
            return null;
        }
        return await CreateTokenResponse(user.Value);
    }

    /// <summary>
    /// Registriert einen neuen User
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<User>?> RegisterAsync(UserDto request)
    {
        _logger.LogInformation("User Registrierungs Prozess wird gestartet für E-Mail: {Email}", request.Email);

        // Prüfe, ob der Benutzername bereits existiert
        if ((await _UserRepository.ExistsByUsernameAsync(request.Username)).Value)
        {
            _logger.LogWarning("Benutzername {Username} existiert bereits", request.Username);
            return Result.Failure<User>(BaseError.BadRequest("UsernameExistiertBereits", "Der Username existiert bereits."));
        }

        var hashedPassword = _passwordService.HashPassword(request.Password);

        // Erstelle einen neuen Benutzer u Mappe ihn mit AutoMapper
        var user = _mapper.Map<User>(request);

        if (user == null)
        {
            return null!;
        }

        // Passwort hashen und setzen (wird nicht durch AutoMapper gemappt)
        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, request.Password);

        // Füge den Benutzer zur Datenbank hinzu und speichere die Änderungen
        var createdUser = await _UserRepository.CreateAsync(user);

        if (createdUser!.IsSuccess)
        {
            return createdUser;
        }
        else
        {
            _logger.LogError("Fehler beim Erstellen des Benutzers: {Error}", createdUser.Error);
            return Result.Failure<User>(BaseError.InternalServerError("BenutzerErstellungFehlgeschlagen", "Fehler beim Erstellen des Benutzers."));
        }
    }
}
