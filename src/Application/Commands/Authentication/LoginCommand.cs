using System;
using Application.Authentication;
using Application.Interfaces;
using Application.Interfaces.Services;
using Artikelsystem.Shared.DTOs.User.Request;
using Domain.Common.BaseErrors;
using Domain.Common.ResultPattern;
using Domain.Entities.Authentication;
using Domain.Errors;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Authentication;

public class LoginCommand : UserLoginDto, IRequest<Result<TokenResponseDto>>
{

}

public class LoginCommandValidation : AbstractValidator<LoginCommand>
{
    private readonly IUserService _userService;
    public LoginCommandValidation(IUserService userService)
    {
        _userService = userService;
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Benutzername ist erforderlich.")
            .MaximumLength(50)
            .WithMessage("Benutzername darf maximal 50 Zeichen lang sein.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Passwort ist erforderlich.")
            .MinimumLength(6)
            .WithMessage("Passwort muss mindestens 6 Zeichen lang sein.");
    }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<TokenResponseDto>>
{
    private readonly IUserService _userService;
    private readonly IPasswordService _passwordService;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly ILogger<LoginCommandHandler> _logger;
    public LoginCommandHandler(
        IAuthenticationService authenticationService,
        IUserService userService,
        IPasswordService passwordService,
        IJwtTokenGenerator tokenGenerator,
        ILogger<LoginCommandHandler> logger
        )
    {
        _userService = userService;
        _passwordService = passwordService;
        _logger = logger;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<Result<TokenResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login für Benutzer {Username} gestartet", request.Username);
        try 
        {
            var user = await _userService.GetByUserNameAsync(request.Username);
            if(user is null || user.Value is null)
            {
                _logger.LogInformation("Login fehlgeschlagen: Benutzername {Username} existiert nicht", request.Username);
                return Result<TokenResponseDto>.Failure(UserError.InvalideCredentials());
            }

            if(!_passwordService.VerifyPassword(user.Value, request.Password))
            {
                _logger.LogWarning("Login fehlgeschlagen: Falsches Passwort für Benutzername {Username}", request.Username);
                return Result<TokenResponseDto>.Failure(UserError.InvalideCredentials());
            }

            // Token generieren
            var accessToken = await _tokenGenerator.CreateAccessTokenAsync(user.Value);
            var refreshToken = await _tokenGenerator.GenerateAndSaveRefreshTokenAsync(user.Value);

            _logger.LogInformation("Login erfolgreich für Benutzername {Username}", request.Username);

            return Result<TokenResponseDto>.Success(new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
        catch (Exception ex)
        {
            return Result<TokenResponseDto>.Failure(UserError.NichtDefinierterFehler(ex.Message));
        }
    }
}