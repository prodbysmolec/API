using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using Application.Interfaces.Repositories;
using Artikelsystem.Shared.DTOs.Artikel.Enums;
using Artikelsystem.Shared.DTOs.Artikel.Request;
using Domain.Common.ResultPattern;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Artikel;

public class CreateArtikelCommand : CreateArtikelRequest, IRequest<Result<bool>>
{
}

public class CreateArtikelCommandValidator : AbstractValidator<CreateArtikelCommand>
{
    private readonly IArtikelGruppeRepository _artikelGruppeRepository;
    public CreateArtikelCommandValidator(IArtikelGruppeRepository artikelGruppeRepository)
    {
        _artikelGruppeRepository = artikelGruppeRepository;
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name ist erforderlich.")
            .MaximumLength(100);

        RuleFor(x => x.Preis)
            .NotEmpty()
            .WithMessage("Preis ist erforderlich.")
            .GreaterThan(0)
            .WithMessage("Preis muss größer als 0 sein.");

        RuleFor(x => x.Mindestbestand)
            .NotEmpty()
            .WithMessage("Mindestbestand ist erforderlich.")
            .GreaterThan(0)
            .WithMessage("Mindestbestand muss größer als 0 sein.");

        RuleFor(x => x.Maximalbestand)
            .NotEmpty()
            .WithMessage("Maximalbestand ist erforderlich.")
            .GreaterThan(0)
            .WithMessage("Maximalbestand muss größer als 0 sein.")
            .GreaterThan(x => x.Mindestbestand)
            .WithMessage("Maximalbestand muss größer als Mindestbestand sein.");
            
        RuleFor(x => x.Menge)
            .NotEmpty()
            .WithMessage("Menge ist erforderlich.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Menge muss größer oder gleich 0 sein.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Status ist ungültig. Erlaubte Werte sind: Verfügbar, Ausverkauft, Vorbestellung.");

        RuleFor(x => x.ArtikelGruppeId)
        // prüfe, ob ArtikelGruppeId existiert
            .NotEmpty()
            .WithMessage("ArtikelGruppeId ist erforderlich.")
            .GreaterThan(0)
            .WithMessage("ArtikelGruppeId muss größer als 0 sein.")
            // Prüfe ob es in der DB existiert
            .MustAsync(async (id, cancellation) => await _artikelGruppeRepository.ExistsAsync(id))
            .WithMessage("ArtikelGruppe existiert nicht");
    }
}