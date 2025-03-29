using WebApplication1;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
var employees = new List<Employee>
{
    new Employee { Id = 1, FirstName = "John", LastName = "Doe", 
                Benefits = new List<EmployeeBenefits>
                    {
                        new EmployeeBenefits { BenefitType = BenefitType.Health, Cost = 100 },
                        new EmployeeBenefits { BenefitType = BenefitType.Dental, Cost = 50 }
                    } },
    new Employee { Id = 2, FirstName = "Jane", LastName = "Doe" }
};

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "WebApplication1.xml"));
});

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseNpgsql("Host=localhost;Port=5432;Username=Admin;Database=Test01");
});

builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers(options => {
    options.Filters.Add<FluentValidationFilter>();
});

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
        // Verbindung zur Datenbank testen
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
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program {}