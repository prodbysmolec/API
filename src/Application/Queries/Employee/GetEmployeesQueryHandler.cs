using System;
using Application.DTOs.Employee;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Common.ResultPattern;
using Domain.Errors;
using MediatR;

namespace Application.Queries.Employee;

public class GetEmployeesQueryHandler(IEmployeeService service, IMapper mapper) : IRequestHandler<GetEmployeesQuery, Result<EmployeeListContainerDto>>
{
    private readonly IEmployeeService _service = service;
    private readonly IMapper _mapper = mapper;
    public async Task<Result<EmployeeListContainerDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        try {
            // 1. Alle Employees mit Paging und optionalen Filtern aus der Datenbank laden
            var pagedResult = await _service.GetAllEmployeesAsync(
                request.Page,
                request.RecordsPerPage,
                request.FirstNameContains,
                request.LastNameContains
            );
            // 2. Prüfen, ob Ergebnisse vorhanden sind
            if(pagedResult.Items == null || !pagedResult.Items.Any())
            {
                return await Task.FromResult(Result<EmployeeListContainerDto>.Failure(EmployeeErrors.EmployeesNotFound()));
            }
            // 3. Employees in Dto umwandeln
            var employeeDtos = _mapper.Map<List<EmployeeListDto>>(pagedResult.Items);
            // 4. PagedResultDto in EmployeeListContainerDto umwandeln
            var employeeListContainer = new EmployeeListContainerDto
            {
                Employees = employeeDtos,
                Page = pagedResult.Page,
                RecordsPerPage = pagedResult.RecordsPerPage,
                TotalRecords = pagedResult.TotalRecords,
                TotalPages = pagedResult.TotalPages
            };

            // 5. Dto zurückgeben
            return Result<EmployeeListContainerDto>.Success(employeeListContainer);
        }
        catch
        {
            // 5. Fehler zurückgeben
            return await Task.FromResult(Result<EmployeeListContainerDto>.Failure(EmployeeErrors.EmployeesNotFound()));
        }
    }
}
