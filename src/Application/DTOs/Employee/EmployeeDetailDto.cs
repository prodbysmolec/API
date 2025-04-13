using System;

namespace Application.DTOs.Employee;

public record EmployeeDetailDto(
    int Id,
    string FirstName,
    string LastName,
    string? SocialSecurityNumber,
    string? Address1,
    string? Address2,
    string? City,
    string? State,
    string? ZipCode,
    string? PhoneNumber,
    string? Email,
    DateTime ErstelltAm,
    DateTime GeaendertAm,
    string ErstelltVon = "",
    string GeaendertVon = ""
);