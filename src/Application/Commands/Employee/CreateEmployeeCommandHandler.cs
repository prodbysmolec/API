using System;
using Application.Interfaces.Repositories;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Common.ResultPattern;
using Domain.Entities.Employees;
using MediatR;

namespace Application.Commands.Employee;

public class CreateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateEmployeeCommand, Result<int>>
{
    private readonly IUnitOfWork _unitofwork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    public async Task<Result<int>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        // 1. DTO -> Entity
        var employee = _mapper.Map<Domain.Entities.Employees.Employee>(request);

        // 2. Entity in DbContext speichern
        await _unitofwork.EmployeeRepository.AddAsync(employee);

        // 3. UnitOfWork speichern
        await _unitofwork.CommitAsync(cancellationToken);

        // 4. Id zurückgeben
        return Result<int>.Success(employee.Id);
    }
}
