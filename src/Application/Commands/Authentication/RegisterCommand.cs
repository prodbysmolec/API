using System;
using Application.Authentication;
using Application.Interfaces.Repositories;
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

public class RegisterCommand : UserDto, IRequest<Result<User>>
{

}

public class RegisterCommandValidation : AbstractValidator<RegisterCommand>
{
    private readonly IUserRepository _userRepository;
    public RegisterCommandValidation(IUserRepository UserRepository)
    {
        _userRepository = UserRepository;
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

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("E-Mail ist erforderlich.")
            .EmailAddress()
            .WithMessage("Ungültige E-Mail-Adresse.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Vorname ist erforderlich.")
            .MaximumLength(50)
            .WithMessage("Vorname darf maximal 50 Zeichen lang sein.");

        RuleFor(x => x.Nachname)
            .NotEmpty()
            .WithMessage("Nachname ist erforderlich.")
            .MaximumLength(50)
            .WithMessage("Nachname darf maximal 50 Zeichen lang sein.");

        // prüfe, ob der Benutzername bereits existiert
        RuleFor(x => x.Username)
            .MustAsync(async (username, cancellation) =>
            {
                var existsResult = await _userRepository.ExistsByUsernameAsync(username);
                return !existsResult.IsSuccess || !existsResult.Value;
            })
            .WithMessage("Benutzername ist bereits vergeben.");

        RuleFor(x => x.Email)
            .MustAsync(async (email, cancellation) =>
            {
                var existsResult = await _userRepository.ExistsByEmailAsync(email!);
                return !existsResult.IsSuccess || !existsResult.Value;
            })
            .WithMessage("E-Mail Adresse ist bereits vergeben.");   
    }
} 

public class RegisterCommandHandler(
    IUserRepository UserRepository,
    ILogger<RegisterCommandHandler> logger,
    IPasswordService passwordService
) : IRequestHandler<RegisterCommand, Result<User>>
{
    private readonly ILogger<RegisterCommandHandler> _logger = logger;
    private readonly IUserRepository _UserRepository = UserRepository;
    private readonly IPasswordService _passwordService = passwordService;

    public async Task<Result<User>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try 
        {
        _logger.LogInformation("User Registrierungs Prozess wird gestartet für E-Mail: {Email}", request.Email);
        
        // Prüfe, ob der Benutzername bereits existiert
        var existsResult = _UserRepository.ExistsByUsernameAsync(request.Username).Result;
        if (existsResult.IsSuccess && existsResult.Value)
            {
                _logger.LogWarning("Benutzername {Username} existiert bereits", request.Username);
                return Result.Failure<User>(UserError.UserAlreadyExists(request.Username));
            }
        
        // Passwort hashen
        var hashedPassword = _passwordService.HashPassword(request.Password);

        // Erstelle einen neuen Benutzer
        var user = new User
        {
            UserName = request.Username,
            Name = request.Name ?? string.Empty,
            Nachname = request.Nachname ?? string.Empty,
            Email = request.Email ?? string.Empty,
            PasswordHash = hashedPassword
        };
        
        // Füge den Benutzer zur Datenbank hinzu und speichere die Änderungen
        var createdUser = await _UserRepository.CreateAsync(user);
                
        if (!createdUser!.IsSuccess)
        {
            _logger.LogError("Fehler beim Erstellen des Benutzers: {Error}", createdUser.Error);
            return Result.Failure<User>(UserError.UserCreationFailed(createdUser.Value.UserName));
        }    

        return createdUser;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unerwarteter Fehler bei der Benutzerregistrierung");
            return Result.Failure<User>(BaseError.InternalServerError(
                "UnerwarteterFehler", 
                "Ein unerwarteter Fehler ist aufgetreten."));
        }
    }
}
