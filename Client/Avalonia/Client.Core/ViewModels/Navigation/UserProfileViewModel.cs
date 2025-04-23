using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client.Core.ViewModels.Navigation;

public partial class UserProfileViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _displayName;
    [ObservableProperty]
    private string _email;
    [ObservableProperty]
    private string _initials;

    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _nachname;

    [ObservableProperty]
    private int _userId;

    // Benutzerdefinierte Gruppen und Berechtigungen
    [ObservableProperty]
    private List<string> _userGruppen = new();

    [ObservableProperty]
    private List<string> _permissions = new();

    // Hilfseigenschaften
    [ObservableProperty]
    private bool _isAdmin;
    
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
    
    
    public void UpdateProfile(string name, string nachname, string email, string username, 
        int userId, List<string> gruppen, List<string> permissions)
    {
        DisplayName = name;
        Nachname = nachname;
        Email = email;
        Username = username;
        UserId = userId;
        UserGruppen = gruppen;
        Permissions = permissions;
            
        // Admin-Status basierend auf Berechtigungen setzen
        IsAdmin = HasPermission("ADMIN") || UserGruppen.Contains("Administrator");
    }
    
    public bool HasPermission(string permissionCode)
    {
        return Permissions.Contains(permissionCode);
    }
    
    // Methode zum Prüfen der Gruppenmitgliedschaft
    public bool IsInGroup(string groupName)
    {
        return UserGruppen.Contains(groupName);
    }
    public void UpdateProfile(string displayName, string email, string username, bool isAdmin, List<string> roles)
    {
        DisplayName = displayName;
        Email = email;
    }
    
    private void OpenSettings()
    {
        // Implementation to open settings
    }
}
