using System;
using Scalar.AspNetCore;

namespace API.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication UseApplication(this WebApplication app)
    {
        if(app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapScalarApiReference();
            app.MapOpenApi();
        }
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }

    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        // SeedData.MigrateAndSeed(services);
        return app;
    }
}
