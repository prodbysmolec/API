using System;
using Artikelsystem.Api.Features.Employees.Models.DTOs;
using FluentValidation;

namespace Artikelsystem.Api.Features.Employees.Validators;

public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();
            
        RuleFor(x => x.LastName)
            .NotEmpty();
    }   
}