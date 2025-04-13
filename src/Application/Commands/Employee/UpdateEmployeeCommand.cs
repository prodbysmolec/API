using Domain.Common.ResultPattern;
using MediatR;

namespace Application.Commands;

public class UpdateEmployeeCommand : IRequest<Result<bool>>
{
    public int Id { get; init; }
    public string? NewFirstName { get; init; }
    public string? NewLastName { get; init; }
    public string? NewSocialSecurityNumber { get; init; }
    public string? NewAddress1 { get; init; }
    public string? NewAddress2 { get; init; }
    public string? NewCity { get; init; }
    public string? NewState { get; init; }
    public string? NewZipCode { get; init; }
    public string? NewPhoneNumber { get; init; }
    public string? NewEmail { get; init; }
}
