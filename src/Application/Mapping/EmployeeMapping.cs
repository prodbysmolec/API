using System;
using Application.DTOs.Employee;
using Artikelsystem.Shared.DTOs.Employee.Response;
using AutoMapper;
using Domain.Entities.Employees;
namespace Application.Mapping;

public class EmployeeMapping : Profile
{

    public EmployeeMapping()
    { 
        // Entity -> DTO
        CreateMap<Employee, EmployeeDetailDto>();
        CreateMap<Employee, EmployeeListDto>();
        CreateMap<List<Employee>, EmployeeListContainerDto>();

        // DTO -> Entity
        CreateMap<EmployeeDetailDto, Employee>()
            .ForMember(dest => dest.Benefits, opt => opt.Ignore());


    }
}
