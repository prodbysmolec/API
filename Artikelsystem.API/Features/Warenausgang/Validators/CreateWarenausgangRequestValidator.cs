using System;
using Artikelsystem.Api.Features.Warenausgang.Models.DTOs.Requests;
using Artikelsystem.Api.Infrastructure.Persistence.Context;
using FluentValidation;

namespace Artikelsystem.Api.Features.Warenausgang.Validators;

public class CreateWarenausgangRequestValidator : AbstractValidator<CreateWarenausgangRequest>
{
    public CreateWarenausgangRequestValidator()
    {
        RuleFor(x => x.ArtikelPositionen)
            .NotEmpty();
    }
}