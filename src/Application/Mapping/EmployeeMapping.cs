using System;
using Application.Commands;
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
                // Command -> Entity
        CreateMap<CreateEmployeeCommand, Employee>()
            .ForMember(dest => dest.ErstelltAm, opt => opt.MapFrom(_ => DateTime.UtcNow)) // Beispiel für benutzerdefiniertes Mapping
            .ForMember(dest => dest.Benefits, opt => opt.Ignore()); // Ignoriere nicht benötigte Felder

        // DTO -> Entity
        CreateMap<EmployeeDetailDto, Employee>()
            .ForMember(dest => dest.Benefits, opt => opt.Ignore());

    }
}
