using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Client.Core.Services.ApiClient;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client.Core.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        private readonly AuthApiService _authService;

        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _isLoading;

        public event EventHandler LoginSuccessful = delegate { };

        public LoginViewModel(AuthApiService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter username and password";
                return;
            }

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var success = await _authService.LoginAsync(Username, Password);

                if (success)
                {
                    LoginSuccessful?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    ErrorMessage = "Invalid username or password";
                }
            }
            catch (ApiException ex)
            {
                ErrorMessage = $"Login failed: {ex.Message}";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}