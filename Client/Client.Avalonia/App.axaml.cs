using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Client.Core.ViewModels;
using Client.Avalonia.Views;
using System;
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
    private const string AppName = "Artikelsystem";
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

        // Registriere ViewModels
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<LoginViewModel>();

        // Baue den ServiceProvider
        _serviceProvider = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();

            // Zeige LoginWindow als erstes
            var loginWindow = new LoginWindow();
            desktop.MainWindow = loginWindow;

            // Listen for successful login
            if (loginWindow.DataContext is LoginViewModel loginViewModel)
            {
                loginViewModel.LoginSuccessful += (s, e) =>
                {
                    var mainWindow = new MainWindow
                    {
                        DataContext = _serviceProvider.GetRequiredService<MainWindowViewModel>()
                    };
                    desktop.MainWindow = mainWindow;
                    mainWindow.Show();
                    loginWindow.Close();
                };
            }


            //desktop.MainWindow = new MainWindow
            //{
            //    DataContext = new MainWindowViewModel(),
            //};
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