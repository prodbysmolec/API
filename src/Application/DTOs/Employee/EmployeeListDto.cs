namespace Application.DTOs.Employee;

public record EmployeeListDto(
    string FirstName,
    string LastName,
    string? SocialSecurityNumber,
    string? Address1,
    string? Address2,
    string? City,
    string? State,
    string? ZipCode,
    string? PhoneNumber,
    string? Email
);