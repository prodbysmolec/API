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
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}