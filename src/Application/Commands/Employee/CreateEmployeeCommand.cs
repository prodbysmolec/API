using Domain.Common.ResultPattern;
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
