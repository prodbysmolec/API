using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Client.Core.ViewModels;
using Client.Avalonia.Views;
using System;
using Client.Core.Helfer;
using Microsoft.Extensions.DependencyInjection;
using Client.Core.Services.ApiClient;

namespace Client.Avalonia;

public partial class App : Application
{
    private IServiceProvider _serviceProvider;

    // Make services accessible from anywhere in the app
    public static new App Current => (App)Application.Current;
    public IServiceProvider Services => _serviceProvider;

    // API configuration
    private const string ApiBaseUrl = "http://localhost:5058/";
    private const string AppName = "Merchandise System";

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Konfiguriere Services
        var services = new ServiceCollection();

        // Konfiguriere die API Clients
        services.ConfigureApiClient(ApiBaseUrl, AppName);

        // Registriere ViewModels mit korrekten Lebensdauern
        services.AddSingleton<MainWindowViewModel>();  // Wichtig: Als Singleton registrieren!
        services.AddTransient<MainViewModel>();        // MainViewModel kann transient bleiben
        services.AddTransient<LoginViewModel>();       // LoginViewModel kann transient bleiben
        services.AddSingleton<PermissionHelper>();     // PermissionHelper ist bereits Singleton
        services.AddScoped<ArtikelViewModel>();   // ArtikelViewModel ist Scoped, da es von MainViewModel verwendet wird
        services.AddTransient<ArtikelViewModel>(); // ArtikelDetailViewModel kann transient bleiben
        // Baue den ServiceProvider
        _serviceProvider = services.BuildServiceProvider();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();

            desktop.MainWindow = new MainWindow
            {
                DataContext = _serviceProvider.GetRequiredService<MainWindowViewModel>()
            };
        }
        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}