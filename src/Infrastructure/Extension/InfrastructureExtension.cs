using System;
using Application.Authentication;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.UnitOfWork;
using Application.Services;
using Domain.Common;
using Infrastructure.Authentication;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Authentication;
using Infrastructure.UnitOfWork;
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
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IArtikelRepository, ArtikelRepository>();
        services.AddScoped<IArtikelGruppeRepository, ArtikelGruppeRepository>();
        services.AddScoped<IWarenausgangRepository, WarenausgangRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserGruppenRepository, UserGruppenRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();


        services.AddScoped<IWareneingangRepository, WareneingangRepository>();

        return services;
    }
}
