using System;
using Domain.Common.BaseErrors;

namespace Domain.Errors;

public static class UserError
{
    public static BaseError UserAlreadyExists(string username)
    {
        return BaseError.Conflict(
            "User.ExistiertBereits",
            $"Der Benutzer mit dem Benutzernamen {username} existiert bereits."
        );
    }

    public static BaseError UserCreationFailed(string username)
    {
        return BaseError.InternalServerError(
            "User.ErstellungFehlgeschlagen",
            $"Die Erstellung des Benutzers mit dem Benutzernamen {username} ist fehlgeschlagen."
        );
    }

    public static BaseError InvalideCredentials()
    {
        return BaseError.UnAuthorized(
            "Authentication.UngültigeAnmeldeinformationen",
            "Die Anmeldeinformationen sind ungültig."
        );
    }

    public static BaseError NichtDefinierterFehler(string exception)
    {
        return BaseError.UnAuthorized(
            "Authentication.NichtDefinierterFehler",
            "Ein nicht definierter Fehler ist aufgetreten."
        );
    }
}