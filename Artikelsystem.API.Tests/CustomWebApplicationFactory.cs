using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using API;
using API.Infrastructure.Persistence.Context;
using Testcontainers.PostgreSql;
using System.Net.Http.Json;

namespace API.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncDisposable
{
    private readonly PostgreSqlContainer _dbContainer;
    public TestSystemClock SystemClock { get; } = new TestSystemClock();
    
    public CustomWebApplicationFactory()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithDatabase("TestDB")
            .WithUsername("testuser")
            .WithPassword("testpassword")
            .WithCleanUp(true)
            .Build();
        
        _dbContainer.StartAsync().GetAwaiter().GetResult();
    }

    public async Task DeleteAllEntities<T>(HttpClient client, string endpoint) where T : class
        {
    // Fetch all entities from the specified endpoint
    var response = await client.GetAsync(endpoint);
    response.EnsureSuccessStatusCode();
    var entities = await response.Content.ReadFromJsonAsync<List<T>>();

    // Iterate through each entity and delete it
    foreach (var entity in entities!)
        {
        try
            {
            // Assuming the entity has an "Id" property
            var idProperty = entity.GetType().GetProperty("Id");
            if (idProperty == null)
                {
                throw new InvalidOperationException("Entity does not have an 'Id' property.");
                }

            var id = idProperty.GetValue(entity);
            var deleteResponse = await client.DeleteAsync($"{endpoint}/{id}");

            if (deleteResponse.IsSuccessStatusCode)
                {
                Console.WriteLine($"Entity with ID {id} was successfully deleted.");
                }
            else
                {
                Console.WriteLine($"Failed to delete entity with ID {id}. Status code: {deleteResponse.StatusCode}");
                }
            }
        catch (Exception ex)
            {
            Console.WriteLine($"Error deleting entity: {ex.Message}");
            }
        } 
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Load the main program's appsettings.json
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        });

        builder.ConfigureServices((context, services) =>
        {
            // Remove the app's DbContext registration
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            // Use the test container's connection string
            string connectionString = _dbContainer.GetConnectionString();

            // Register test DbContext
            services.AddDbContext<AppDbContext>((container, options) =>
            {
                options.UseNpgsql(connectionString);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            // Replace system clock with test version
            var systemClockDescriptor = services.Single(d => d.ServiceType == typeof(ISystemClock));
            services.Remove(systemClockDescriptor);
            services.AddSingleton<ISystemClock>(SystemClock);

            // Initialize the database
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            // Ensure database is created and schema is applied
            dbContext.Database.EnsureCreated();
            
            // Optionally add test seed data here
            // TestSeedData.Initialize(dbContext);
        });
    }

    public override async ValueTask DisposeAsync()
    {
        // Clean up the container when tests are done
        await _dbContainer.DisposeAsync();
        await base.DisposeAsync();
    }

    public class TestSystemClock : ISystemClock
    {
        private DateTimeOffset _now = DateTimeOffset.Parse("2022-01-01T00:00:00Z");
        
        public DateTimeOffset UtcNow => _now;
        
        public void AdvanceBy(TimeSpan timeSpan)
        {
            _now = _now.Add(timeSpan);
        }
    }
}