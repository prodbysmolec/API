using System;

namespace Artikelsystem.Shared.Constants;

public static class ApiRoutes
{
    public static class Authentication
    {
        private const string Base = "/Authentication";

        public const string Register = $"{Base}/Register";
        public const string Login = $"{Base}/Login";
        public const string AdminOnly = $"{Base}/admin-only";
        public const string AuthenticateOnly = $"{Base}";
    }

    public static class Artikel
    {
        private const string Base = "/Artikel";

        public const string GetAllArtikel = $"{Base}";
        public const string GetArtikelById = $"{Base}/{{id}}";
        public const string CreateArtikel = $"{Base}";
    }

    public static class Employee
    {
        private const string Base = "/Employee";
        public const string GetAllEmployees = $"{Base}";
        public const string GetEmployeeById = $"{Base}/{{id}}";
        public const string CreateEmployee = $"{Base}";
        public const string UpdateEmployee = $"{Base}/{{id}}";
    }
}
