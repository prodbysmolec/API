using System;
using Client.Core.Services.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Core.Services.ApiClient;

public static class ApiClientFactory
{
    public static IServiceCollection ConfigureApiClient(this IServiceCollection services, string baseUrl, string appName)
    {
        // Register secure storage
        services.AddSingleton<ISecureStorage>(sp => new SecureStorageImplementation(appName));

        // Register token service
        services.AddSingleton<ITokenService>(provider =>
            new TokenService(baseUrl, provider.GetRequiredService<ISecureStorage>()));

        // Register base HTTP client
        services.AddSingleton(provider =>
            new HttpClientBase(baseUrl, provider.GetRequiredService<ITokenService>()));

        // Register API services
        services.AddSingleton<AuthApiService>();

        // Add other API services here
        services.AddSingleton<ArtikelApiService>();
        services.AddSingleton<EmployeeApiService>();

        return services;
    }
}
