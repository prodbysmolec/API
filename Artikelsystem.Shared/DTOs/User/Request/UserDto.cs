using System;
using FluentValidation;

namespace Artikelsystem.Shared.DTOs.User.Request;

public class UserDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public string? Name { get; set; }
    public string? Nachname { get; set; }
    public string? Email { get; set; }
}

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty()
                .WithMessage("Der Username darf nicht leer sein.");
            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage("Das Passwort darf nicht leer sein.");
        } 
}