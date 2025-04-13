using System.Security.Claims;
using System.Text;
using Artikelsystem.Shared.DTOs.Inventur;
using API.Shared.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Scalar.AspNetCore;
using Serilog;
using API.Extensions;
using Infrastructure.Extension;
using MediatR;
using Application.Extensions;
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try 
{
    Log.Information("Anwendung wird gestartet.");

    var builder = WebApplication.CreateBuilder(args);
    // Add services to the container.
    builder.AddPresentation();
    builder.AddOpenApiSetup();
    builder.AddValidation();
    builder.Services.AddApplication();
    builder.Services.AddDependencyInjection();
    builder.Services.AddDatabase(builder.Configuration);
    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);
    var app = builder.Build()
        .UseApplication()
        .ApplyMigrations();

    app.Run();
}
catch (Exception ex) 
{
    Log.Fatal(ex, "Die Anwendung konnte nicht gestartet werden.");
    throw;
}
finally 
{
    Log.CloseAndFlush();
}

#region Services DI
// builder.Services.AddScoped<IInventurService, InventurService>();
// builder.Services.AddScoped<IArtikelLieferantService, ArtikelLieferantService>();
// builder.Services.AddScoped<ILieferantService, LieferantService>();
// builder.Services.AddScoped<IAuthService, AuthService>();
// builder.Services.AddScoped<IWarenausgangService, WarenausgangService>();
#endregion


// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     // SeedData.MigrateAndSeed(services);
// }


public partial class Program { }