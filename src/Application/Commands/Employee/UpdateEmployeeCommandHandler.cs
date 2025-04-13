using System;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Common.ResultPattern;
using Domain.Errors;
using MediatR;

namespace Application.Commands.Employee;

public class UpdateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateEmployeeCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitofwork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    public async Task<Result<bool>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        // 1. Bestehenden Employee aus der Datenbank laden
        var existingEmployee = await _unitofwork.EmployeeRepository.GetByIdAsync(request.Id);
        if (existingEmployee == null)
        {
            return Result<bool>.Failure(EmployeeErrors.EmployeeNotFound(request.Id));
        }

        // 2. Nur die geänderten Felder aktualisieren
        _mapper.Map(request, existingEmployee);

        // 3. Entity in DbContext speichern
        await _unitofwork.EmployeeRepository.UpdateAsync(existingEmployee);

        // 4. UnitOfWork speichern
        await _unitofwork.CommitAsync(cancellationToken);

        // 5. Bool zurückgeben
        return Result<bool>.Success(true);
    }
}
