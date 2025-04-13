using System;
using Application.DTOs.Employee;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Common.BaseErrors;
using Domain.Common.ResultPattern;
using Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Artikelsystem.Shared.DTOs.Employee.Response;
using API.Features.Employees.Models.DTOs;
namespace Application.Queries.Employee;

public class GetEmployeeQueryHandler(IEmployeeService service, IMapper mapper) : IRequestHandler<GetEmployeeQuery, Result<GetEmployeeResponse>>
{
    private readonly IEmployeeService _service = service;
    private readonly IMapper _mapper = mapper;
    public async Task<Result<GetEmployeeResponse>> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
    {
        try 
        {
        if(request.Id <= 0)
        {
            return Result<GetEmployeeResponse>.Failure(EmployeeErrors.IdIstNullOderNegativ(request.Id));
        }
        var employeeEntity = await _service.GetEmployeeByIdAsync(request.Id);

        if (employeeEntity == null)
        {
            return Result<GetEmployeeResponse>.Failure(EmployeeErrors.EmployeeNotFound(request.Id));
        }

        var employeeDto = _mapper.Map<GetEmployeeResponse>(employeeEntity);
        return Result<GetEmployeeResponse>.Success(employeeDto);
        }
        catch (Exception)
        {
            return Result<GetEmployeeResponse>.Failure(EmployeeErrors.EmployeeNotFound(request.Id));
        }
    }
}