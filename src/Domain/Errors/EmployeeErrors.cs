using System;
using Domain.Common.BaseErrors;

namespace Domain.Errors;

public static class EmployeeErrors
{
    public static BaseError EmployeeNotFound(int id) 
    {
        return BaseError.NotFound(
            "Employee.NotFound",
            $"Employee mit der ID {id} wurde nicht gefunden."
        );
    }

    public static BaseError EmployeesNotFound() 
    {
        return BaseError.NotFound(
            "Employees.NotFound",
            $"There are no Employees in the Database."
        );
    }

    public static BaseError EMailAlreadyExists(string email) 
    {
        return BaseError.Conflict(
            "Employee.EmailAlreadyExists",
            $"Die E-Mail-Adresse {email} existiert bereits."
        );
    }

    public static BaseError IdIstNullOderNegativ(int id)
    {
        return BaseError.Validation(
            "Employee.IdIstNullOderNegativ",
            $"Die ID {id} darf nicht 0 oder negativ sein."
        );
    }
}
