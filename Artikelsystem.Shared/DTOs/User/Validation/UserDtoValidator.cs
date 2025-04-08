using System;
using Artikelsystem.Shared.DTOs.User.Request;
using FluentValidation;

namespace Artikelsystem.Shared.DTOs.User.Validation;

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
