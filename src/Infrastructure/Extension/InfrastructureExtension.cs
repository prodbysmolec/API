using System;
using Application.Authentication;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Application.Services;
using Infrastructure.Authentication;
using Infrastructure.Common.UnitOfWork;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;

namespace Infrastructure.Extension;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<ISystemClock, SystemClock>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IArtikelService, ArtikelService>();
        services.AddScoped<IArtikelGruppeService, ArtikelGruppeService>();
        services.AddScoped<IWarenausgangService, WarenausgangService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserGruppenService, UserGruppenService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IPasswordService, PasswordService>();




        services.AddScoped<IWareneingangService, WareneingangService>();

        return services;
    }
}
