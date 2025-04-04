using System;
using Artikelsystem.Api.Features.Inventur.Models.Dtos;
using FluentValidation;

namespace Artikelsystem.Api.Features.Inventur.Validators;

public class UpdateInventurPositionRequestValidator : AbstractValidator<UpdateInventurPositionRequest>
{
    public UpdateInventurPositionRequestValidator()
    {
        RuleFor(x => x.PositionId)
            .GreaterThan(0).WithMessage("Die Positions-ID muss größer als 0 sein.");

        RuleFor(x => x.InventurID)
            .GreaterThan(0).WithMessage("Die Inventur-ID muss größer als 0 sein.");

        RuleFor(x => x.ArtikelId)
            .GreaterThan(0).WithMessage("Die Artikel-ID muss größer als 0 sein.");

        RuleFor(x => x.GezaehlteMenge)
            .GreaterThanOrEqualTo(0).WithMessage("Die gezählte Menge darf nicht negativ sein.");

        RuleFor(x => x.BearbeitetVon)
            .NotEmpty().WithMessage("Der Bearbeiter muss angegeben werden.")
            .MaximumLength(100).WithMessage("Der Bearbeitername darf maximal 100 Zeichen lang sein.");

        RuleFor(x => x.Bemerkung)
            .MaximumLength(1000).WithMessage("Die Bemerkung darf maximal 1000 Zeichen lang sein.");
    }
}
