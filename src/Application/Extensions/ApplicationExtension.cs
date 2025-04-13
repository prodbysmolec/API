using System;
using Application.Interfaces.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
namespace Application.Extensions;

public static class ApplicationExtension
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        var applicationAssembly = typeof(ApplicationExtension).Assembly;

        // AutoMapper
        services.AddAutoMapper(applicationAssembly);

        // FluentValidation
        services.AddValidatorsFromAssembly(applicationAssembly, includeInternalTypes: true);

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));

        services.AddScoped<IMediator, Mediator>();
        //services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ApplicationExtension>());
        // services.AddTransient<IValidatorFactory, ServiceProviderValidatorFactory>();
        return services;
    }
}