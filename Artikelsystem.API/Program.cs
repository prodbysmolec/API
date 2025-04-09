using System.Security.Claims;
using System.Text;
using Artikelsystem.Api.Features.Employees.Models.Entitys;
using Artikelsystem.Api.Features.Inventur.Models.Dtos;
using Artikelsystem.Api.Features.Inventur.Services;
using Artikelsystem.Api.Features.Lieferant.Services;
using Artikelsystem.Api.Infrastructure.Persistence.Context;
using Artikelsystem.Api.Infrastructure.Persistence.Repositories;
using Artikelsystem.Api.Infrastructure.Persistence.Seeding;
using Artikelsystem.Api.Shared.Validation;
using Artikelsystem.API.Features.Authentication.Services;
using Artikelsystem.API.Features.Lieferant.Services;
using Artikelsystem.API.Features.Warenausgang.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi("v1");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters 
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
            ValidateIssuerSigningKey = true,
            RoleClaimType = ClaimTypes.Role
        };
    });

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Artikelsystem.Api.xml"));
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
#region Services DI
builder.Services.AddScoped<IInventurService, InventurService>();
builder.Services.AddScoped<IArtikelLieferantService, ArtikelLieferantService>();
builder.Services.AddScoped<ILieferantService, LieferantService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IWarenausgangService, WarenausgangService>();
builder.Services.AddScoped<CreateInventurRequestValidator>();
builder.Services.AddScoped<UpdateInventurPositionRequestValidator>();


#endregion
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationFilter>();
});
builder.Services.AddSingleton<ISystemClock, SystemClock>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.MigrateAndSeed(services);
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        var canConnect = context.Database.CanConnect();
        if (canConnect)
        {
            Console.WriteLine("Erfolgreich mit der Datenbank verbunden!");
        }
        else
        {
            Console.WriteLine("Verbindung zur Datenbank fehlgeschlagen!");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Fehler beim Verbinden mit der Datenbank: {ex.Message}");
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapScalarApiReference();
    app.MapOpenApi();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program { }