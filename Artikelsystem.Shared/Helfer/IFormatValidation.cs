using System;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Artikelsystem.Shared.Helfer;

public interface IFormatValidation
{
    object FormatValidationErrors(ValidationResult validationResult);
}
public class FormatValidation : IFormatValidation
{
    public object FormatValidationErrors(ValidationResult validationResult)
    {
        var errors = validationResult.Errors
            .Select(e => new
            {
                Field = e.PropertyName,
                Message = e.ErrorMessage
            })
            .ToList();

        return new
        {
            Title = "Validation Failed",
            Status = StatusCodes.Status400BadRequest,
            Errors = errors
        };
    }
}
