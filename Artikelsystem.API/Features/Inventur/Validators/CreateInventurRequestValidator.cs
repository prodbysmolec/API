using System;
using Artikelsystem.Api.Features.Inventur.Models.Dtos;
using FluentValidation;

namespace Artikelsystem.Api.Features.Inventur.Validators;

public class CreateInventurRequestValidator : AbstractValidator<CreateInventurRequest>
{
    public CreateInventurRequestValidator()
    {
        RuleFor(x => x.Bezeichnung)
            .NotEmpty().WithMessage("Die Bezeichnung darf nicht leer sein.")
            .MaximumLength(200).WithMessage("Die Bezeichnung darf maximal 200 Zeichen lang sein.");

        RuleFor(x => x.ErstelltVon)
            .NotEmpty().WithMessage("Der Ersteller muss angegeben werden.")
            .MaximumLength(100).WithMessage("Der Erstellername darf maximal 100 Zeichen lang sein.");

        RuleFor(x => x.Bemerkung)
            .MaximumLength(1000).WithMessage("Die Bemerkung darf maximal 1000 Zeichen lang sein.");
    }
}