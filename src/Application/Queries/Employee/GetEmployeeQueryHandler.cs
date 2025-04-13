using System;
using Application.DTOs.Employee;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Common.ResultPattern;
using Domain.Errors;
using MediatR;
namespace Application.Queries.Employee;

public class GetEmployeeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetEmployeeQuery, Result<EmployeeDetailDto>>
{
    private readonly IUnitOfWork _unitofwork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    public async Task<Result<EmployeeDetailDto>> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
    {
        // 1. Dto -> Entity
        var employeeEntity = await _unitofwork.EmployeeRepository.GetByIdAsync(request.Id);

        if (employeeEntity == null)
        {
            return Result<EmployeeDetailDto>.Failure(EmployeeErrors.EmployeeNotFound(request.Id));
        }
        
        // 3. Entity -> Dto
        var employeeDto = _mapper.Map<EmployeeDetailDto>(employeeEntity);

        // 4. Dto zurückgeben
        return Result<EmployeeDetailDto>.Success(employeeDto);
    }
}