using System;
using Application.DTOs.Employee;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Common.ResultPattern;
using Domain.Errors;
using MediatR;

namespace Application.Queries.Employee;

public class GetEmployeesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetEmployeesQuery, Result<EmployeeListContainerDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    public async Task<Result<EmployeeListContainerDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        try {
            // 1. Alle Employees aus der Datenbank laden
            var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            // 2. Wenn keine Employees gefunden wurden, Fehler zurückgeben
            if (employees == null || !employees.Any())
            {
                return await Task.FromResult(Result<EmployeeListContainerDto>.Failure(EmployeeErrors.EmployeesNotFound()));
            }

            // 3. Employees in Dto umwandeln
            var employeeDtos = _mapper.Map<List<EmployeeListDto>>(employees);
            var employeeListContainer = new EmployeeListContainerDto
            {
                Employees = employeeDtos
            };

            // 4. Dto zurückgeben
            return await Task.FromResult(Result<EmployeeListContainerDto>.Success(employeeListContainer));
        }
        catch
        {
            // 5. Fehler zurückgeben
            return await Task.FromResult(Result<EmployeeListContainerDto>.Failure(EmployeeErrors.EmployeesNotFound()));
        }

    }
}
