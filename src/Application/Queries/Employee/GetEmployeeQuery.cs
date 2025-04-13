using API.Features.Employees.Models.DTOs;
using Application.DTOs.Employee;
using Domain.Common.ResultPattern;
using MediatR;

namespace Application.Queries;

public record GetEmployeeQuery(int Id) : IRequest<Result<GetEmployeeResponse>>;
