using Application.Interfaces.UnitOfWork;
using Domain.Common.ResultPattern;
using FluentValidation;
using MediatR;

namespace Application.Commands;

public class CreateEmployeeCommand : IRequest<Result<int>>
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string? SocialSecurityNumber { get; init; }
    public string? Address1 { get; init; }
    public string? Address2 { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? ZipCode { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Email { get; init; }
}

// Beispiel validierungen
public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    public CreateEmployeeCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("A valid email address is required.")
            // validiere dass email nur 1x in der db vorkommt.
            .MustAsync(async (email, cancellation) =>
            {
                var existiert = await _unitOfWork.EmployeeRepository.EmailExistsAsync(email);
                if (existiert.IsFailure)
                {
                    return true; // Email already exists
                }

                return false;
            }).WithMessage("Es existiert bereits ein Employee mit dieser E-Mail Adresse.")
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}
