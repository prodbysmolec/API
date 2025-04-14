using System;
using Domain.Common.BaseErrors;

namespace Domain.Errors;

public class ArtikelErrors
{
    public static BaseError ArtikelNotFound() 
    {
        return BaseError.NotFound(
            "Artikel.NotFound",
            $"Es existieren keine Artikel in der Datenbank."
        );
    }
}
