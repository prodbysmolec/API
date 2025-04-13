using Application.DTOs.Employee;
using Domain.Common.ResultPattern;
using MediatR;

namespace Application.Queries.Employee;

public record GetEmployeesQuery(
    int Page = 1,
    int RecordsPerPage = 10,
    string? FirstNameContains = null,
    string? LastNameContains = null
) : IRequest<Result<EmployeeListContainerDto>>;