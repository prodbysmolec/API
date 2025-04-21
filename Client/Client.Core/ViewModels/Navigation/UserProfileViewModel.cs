using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client.Core.ViewModels.Navigation;

public class UserProfileViewModel : ViewModelBase
{
    private string _displayName;
    private string _email;
    private string _initials;
    
    public string DisplayName
    {
        get => _displayName;
        set => SetProperty(ref _displayName, value);
    }
    
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }
    
    public string Initials
    {
        get => _initials;
        set => SetProperty(ref _initials, value);
    }
    
    public IRelayCommand OpenSettingsCommand { get; }
    
    public UserProfileViewModel(string displayName, string email)
    {
        _displayName = displayName;
        _email = email;
        
        // Generate initials from name
        if (!string.IsNullOrEmpty(displayName))
        {
            var nameParts = displayName.Split(' ');
            if (nameParts.Length > 1)
            {
                _initials = $"{nameParts[0][0]}{nameParts[1][0]}";
            }
            else if (nameParts.Length == 1 && nameParts[0].Length > 0)
            {
                _initials = nameParts[0][0].ToString();
            }
            else
            {
                _initials = "?";
            }
        }
        
        OpenSettingsCommand = new RelayCommand(OpenSettings);
    }
    
    private void OpenSettings()
    {
        // Implementation to open settings
    }
}
