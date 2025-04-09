using Artikelsystem.Shared.DTOs.Warenausgang.Enums;
using FluentValidation;

namespace Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Request;

public class CreateWarenausgangDto
{
    public WarenausgangZweckEnum Zweck { get; set; }
    public string? AllgemeineBemerkungen { get; set; }
    public List<CreateWarenausgangArtikelPositionDto> ArtikelPositionen { get; set; } = new();
}

public class CreateWarenausgangDtoValidation : AbstractValidator<CreateWarenausgangDto>
{
    public CreateWarenausgangDtoValidation()
    {
        RuleFor(x => x.Zweck)
            .NotEqual(WarenausgangZweckEnum.None)
            .WithMessage("Der Zweck darf nicht 'None' sein.");

        RuleFor(x => x.AllgemeineBemerkungen)
            .MaximumLength(500)
            .WithMessage("Die allgemeinen Bemerkungen dürfen maximal 500 Zeichen lang sein.");

        RuleForEach(x => x.ArtikelPositionen)
            .SetValidator(new CreateWarenausgangArtikelPositionDtoValidator());

        // Rechnungsnummer und Verkaufspreis dürfen nur gesetzt sein, wenn es Bestellung ist
        When(x => x.Zweck != WarenausgangZweckEnum.Bestellung, () =>
        {
            RuleFor(x => x.ArtikelPositionen)
                .Must(x => x.All(p => p.Rechnungsnummer == null))
                .WithMessage("Die Rechnungsnummer darf nicht gesetzt sein, wenn der Zweck nicht 'Bestellung' ist.");

            RuleFor(x => x.ArtikelPositionen)
                .Must(x => x.All(p => p.Verkaufspreis == null))
                .WithMessage("Der Verkaufspreis darf nicht gesetzt sein, wenn der Zweck nicht 'Bestellung' ist.");
        });

        // Wenn WarenausgangZweckEnum.Werbegeschenk dann darf von ArtikelPositionen Bemerkung nicht leer sein
        When(x => x.Zweck == WarenausgangZweckEnum.Werbegeschenk, () =>
        {
            RuleFor(x => x.ArtikelPositionen)
                .Must(x => x.All(p => !string.IsNullOrEmpty(p.Bemerkung)))
                .WithMessage("Die Bemerkung darf nicht leer sein, wenn der Zweck 'Werbegeschenk' ist.");

            RuleFor(x => x.ArtikelPositionen)
                .Must(x => x.All(p => p.Verkaufspreis == null))
                .WithMessage("Der Verkaufspreis darf nicht gesetzt sein, wenn der Zweck 'Werbegeschenk' ist.");

            RuleFor(x => x.ArtikelPositionen)
                .Must(x => x.All(p => p.Rechnungsnummer == null))
                .WithMessage("Die Rechnungsnummer darf nicht gesetzt sein, wenn der Zweck 'Werbegeschenk' ist.");
        });
    }
}

public class CreateWarenausgangArtikelPositionDto
{
    public int ArtikelId { get; set; }
    public int Menge { get; set; }
    public string Bemerkung { get; set; } = string.Empty;
    public decimal? Verkaufspreis { get; set; }
    public string Rechnungsnummer { get; set; } = string.Empty;
}

public class CreateWarenausgangArtikelPositionDtoValidator : AbstractValidator<CreateWarenausgangArtikelPositionDto>
{
    public CreateWarenausgangArtikelPositionDtoValidator()
    {
        RuleFor(x => x.ArtikelId)
            .NotEqual(0)
            .NotEmpty();
        RuleFor(x => x.Menge)
            .NotEmpty()
            .WithMessage("Die Menge darf nicht leer sein.");
        RuleFor(x => x.Bemerkung)
            .MaximumLength(500)
            .WithMessage("Die Bemerkung darf maximal 500 Zeichen lang sein.");
        
        RuleFor(x => x.Rechnungsnummer)
            .NotEmpty()
            .WithMessage("Die Rechnungsnummer darf nicht leer sein.");
    }
}
