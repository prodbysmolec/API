using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Client.Core.Services.ApiClient;
using ReactiveUI;

namespace Client.Core.ViewModels
{
    public class LoginViewModel : ReactiveObject
    {
        private readonly AuthApiService _authService;
        
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isLoading;
        
        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }
        
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }
        
        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }
        
        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }
        
        public ICommand LoginCommand { get; }
        
        public event EventHandler LoginSuccessful = delegate { };
        
        public LoginViewModel(AuthApiService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            LoginCommand = ReactiveCommand.CreateFromTask(LoginAsync);
        }
        
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
                ErrorMessage = null;
                
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