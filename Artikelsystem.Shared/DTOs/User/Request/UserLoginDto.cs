using System;
using FluentValidation;

namespace Artikelsystem.Shared.DTOs.User.Request;

public class UserLoginDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class UserLoginDtoValidator : AbstractValidator<UserDto>
{
    public UserLoginDtoValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty()
                .WithMessage("Der Username darf nicht leer sein.");
            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage("Das Passwort darf nicht leer sein.");
        } 
}