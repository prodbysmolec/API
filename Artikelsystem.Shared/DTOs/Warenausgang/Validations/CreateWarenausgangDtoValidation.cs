using System;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Request;
using Artikelsystem.Shared.DTOs.Warenausgang.Enums;
using FluentValidation;

namespace Artikelsystem.Shared.DTOs.Warenausgang.Validations;

public class CreateWarenausgangDtoValidation : AbstractValidator<CreateWarenausgangDto>
{
    public CreateWarenausgangDtoValidation()
    {
        // erstelle eine validierung für Zweck, Zweck darf nicht 0 sein
        RuleFor(x => x.Zweck)
            .NotEqual(WarenausgangZweckEnum.None)
            .WithMessage("Der ausgewählte Wareneingang darf nicht None sein.");
    }
}
