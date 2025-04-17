using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client.Core.ViewModels;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Avalonia.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            // Service vom App.Services holen
            var loginViewModel = ((App)Application.Current).Services.GetRequiredService<LoginViewModel>();
            DataContext = loginViewModel;
        
            // Event-Handler für erfolgreichen Login
            loginViewModel.LoginSuccessful += OnLoginSuccessful;
        }
        private void OnLoginSuccessful(object sender, EventArgs e)
        {
            // Fenster schließen nach erfolgreichem Login
            Close();
        }
            
        protected override void OnClosed(EventArgs e)
        {
            // Event-Handler entfernen, um Memory-Leaks zu vermeiden
            if (DataContext is LoginViewModel vm)
            {
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
                vm.LoginSuccessful -= OnLoginSuccessful;
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            }
            base.OnClosed(e);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}