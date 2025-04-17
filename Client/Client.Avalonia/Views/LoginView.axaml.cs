using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace Client.Avalonia.Views
{
    public partial class LoginView : UserControl
    {
        private TextBox _passwordTextBox;
        private Button _showPasswordButton;

        public LoginView()
        {
            InitializeComponent();
            InitializeControls();
            SetupEventHandlers();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void InitializeControls()
        {
            _passwordTextBox = this.FindControl<TextBox>("PasswordTextBox");
            _showPasswordButton = this.FindControl<Button>("ShowPasswordButton");
        }

        private void SetupEventHandlers()
        {
            // Toggle password visibility
            _showPasswordButton.Click += (s, e) => TogglePasswordVisibility();
        }

        private void TogglePasswordVisibility()
        {
            // Toggle between showing password and hiding it
            if (_passwordTextBox.PasswordChar == '•')
            {
                _passwordTextBox.PasswordChar = '\0'; // Show password
            }
            else
            {
                _passwordTextBox.PasswordChar = '•'; // Hide password
            }
        }
    }
}