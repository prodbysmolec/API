using Application.DTOs.Employee;
using Domain.Common.ResultPattern;
using MediatR;

namespace Application.Queries.Employee;

public record GetEmployeesQuery : IRequest<Result<EmployeeListContainerDto>>;