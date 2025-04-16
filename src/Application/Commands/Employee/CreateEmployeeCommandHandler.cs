using System;
using Application.Interfaces.Repositories;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Common.ResultPattern;
using Domain.Entities.Employees;
using Domain.Errors;
using MediatR;

namespace Application.Commands.Employee;

public class CreateEmployeeCommandHandler(IEmployeeRepository service, IMapper mapper) : IRequestHandler<CreateEmployeeCommand, Result<int>>
{
    private readonly IEmployeeRepository _service = service;
    private readonly IMapper _mapper = mapper;
    public async Task<Result<int>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        try {
        // 1. DTO -> Entity
        var employee = _mapper.Map<Domain.Entities.Employees.Employee>(request);

        // 2. Entity in DbContext speichern
        await _service.AddEmployeeAsync(employee);

        // 4. Id zurückgeben
        return Result<int>.Success(employee.Id);
        }
        catch
        {
            // Hier können Sie den Fehler protokollieren oder behandeln
            return Result<int>.Failure(EmployeeErrors.EmployeeNotFound(1));
        }   
    }
}
