using System;
using API.Shared.Validation;
using Domain.Options;
using FluentValidation;
using Infrastructure.Extension;
using MediatR;
using Microsoft.Extensions.Internal;
using Serilog;
namespace API.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "API.xml"));
        });

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<ISystemClock, SystemClock>();
        builder.Services.AddProblemDetails();

        // Registering the IOptions
        builder.Services.Configure<ConnStringOptions>(
            builder.Configuration.GetSection(ConnStringOptions.ConnectionStrings));
    }

    public static void AddOpenApiSetup(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi("v1");
    }

    public static void AddValidation(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddControllers(options => 
        {
            options.Filters.Add<FluentValidationFilter>();
        });
        //builder.Services.AddScoped<CreateInventurRequestValidator>();
        //builder.Services.AddScoped<UpdateInventurPositionRequestValidator>();
    }
    
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {

        return services;
    }
}
