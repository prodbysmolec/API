using System;
using API.Features.Employees.Models.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Common.ResultPattern;
using Domain.Errors;
using MediatR;

namespace Application.Commands.Employee;

public class UpdateEmployeeCommandHandler(IEmployeeRepository service, IMapper mapper) : IRequestHandler<UpdateEmployeeCommand, Result<bool>>
{
    private readonly IEmployeeRepository _service = service;
    private readonly IMapper _mapper = mapper;
    public async Task<Result<bool>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        // 1. Bestehenden Employee aus der Datenbank laden
        var existingEmployee = await _service.GetEmployeeByIdAsync(request.Id);
        if (existingEmployee == null)
        {
            return Result<bool>.Failure(EmployeeErrors.EmployeeNotFound(request.Id));
        }

        // 2. Nur die geänderten Felder aktualisieren
        var changedEmployee = _service.UpdateEmployeeAsync(existingEmployee.Id, new UpdateEmployeeRequest
        {
            Address1 = request.NewAddress1,
            Address2 = request.NewAddress2,
            City = request.NewCity,
            State = request.NewState,
            ZipCode = request.NewZipCode,
            PhoneNumber = request.NewPhoneNumber,
            Email = request.NewEmail
        });

        if(changedEmployee == null)
        {
            return Result<bool>.Failure(EmployeeErrors.EmployeeNotFound(request.Id));
        }

        // 5. Bool zurückgeben
        return Result<bool>.Success(true);
    }
}
