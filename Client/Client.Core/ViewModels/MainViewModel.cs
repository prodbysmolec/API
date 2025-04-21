using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Client.Core.Services.Auth;
using Client.Core.ViewModels.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client.Core.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly ITokenService _tokenService;

    [ObservableProperty]
    private ViewModelBase _currentView;

    [ObservableProperty]
    private string _currentPageTitle = "Artikel";

    [ObservableProperty]
    private string _currentPagePath = "Übersicht";

    public ObservableCollection<NavigationItemViewModel> NavigationItems { get; } = new ObservableCollection<NavigationItemViewModel>();

    [ObservableProperty]
    private UserProfileViewModel _userProfile;

    // Manuelles Deklarieren der Commands für Navigation
    public IRelayCommand SearchCommand { get; }
    public IRelayCommand NotificationsCommand { get; }
    public IRelayCommand HelpCommand { get; }
    public IRelayCommand RefreshUserProfileCommand { get; }

    public MainViewModel(ITokenService tokenService)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));

        // Initialize user profile with default values
        UserProfile = new UserProfileViewModel("Lädt...", "");

        // Commands manuell initialisieren
        SearchCommand = new RelayCommand(ExecuteSearch);
        NotificationsCommand = new RelayCommand(OpenNotifications);
        HelpCommand = new RelayCommand(OpenHelp);
        RefreshUserProfileCommand = new AsyncRelayCommand(RefreshUserProfileAsync);

        // Initialize navigation items
        InitializeNavigationItems();

        // Initial user profile load
        _ = RefreshUserProfileAsync();
    }

    private void InitializeNavigationItems()
    {
        // Basic navigation item that everyone should see
        NavigationItems.Add(new NavigationItemViewModel("Artikel",
            "M20 2H4C3 2 2 2.9 2 4V7.01C2 7.73 2.43 8.35 3 8.7V20C3 21.1 4.1 22 5 22H19C19.9 22 21 21.1 21 20V8.7C21.57 8.35 22 7.73 22 7.01V4C22 2.9 21 2 20 2ZM19 20H5V9H19V20ZM20 7H4V4H20V7Z M13 12H16V14H13V12Z M9 12H12V14H9V12Z M13 15H16V17H13V15Z M9 15H12V17H9V15Z",
            new RelayCommand(() => NavigateTo("Artikel", "Liste")),
            true));
    }

    // Entferne RelayCommand Attribut für Methoden mit mehreren Parametern
    public async Task RefreshUserProfileAsync()
    {
        try
        {
            // Get the access token
            var token = await _tokenService.GetAccessTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                // No valid token, use default values
                UserProfile.UpdateProfile("Nicht angemeldet", "", "", "", 0, new List<string>(), new List<string>());
                return;
            }

            // Parse JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Extract user data from claims
            var userId = ExtractClaimValue(jwtToken, "userId", "sub");
            var name = ExtractClaimValue(jwtToken, ClaimTypes.Name, "name", "given_name");
            var nachname = ExtractClaimValue(jwtToken, ClaimTypes.Surname, "family_name", "nachname");
            var email = ExtractClaimValue(jwtToken, ClaimTypes.Email, "email");
            var username = ExtractClaimValue(jwtToken, "preferred_username", "username");

            // Extract user groups
            var userGruppen = ExtractArrayClaim(jwtToken, ClaimTypes.Role, "groups");

            // Extract permissions
            var permissions = ExtractArrayClaim(jwtToken, "permission", "scope");

            // User ID conversion (if not numeric, use 0)
            int parsedUserId = 0;
            int.TryParse(userId, out parsedUserId);

            // Update user profile
            UserProfile.UpdateProfile(
                name ?? "Unbekannt",
                nachname ?? "",
                email ?? "",
                username ?? "",
                parsedUserId,
                userGruppen,
                permissions);

            // Update navigation based on permissions
            UpdateNavigationBasedOnPermissions();
        }
        catch (Exception ex)
        {
            // Error handling
            UserProfile.UpdateProfile("Fehler beim Laden", "", "", "", 0, new List<string>(), new List<string>());
            Console.WriteLine($"Fehler beim Laden des Benutzerprofils: {ex.Message}");
        }
    }

    private string? ExtractClaimValue(JwtSecurityToken token, params string[] possibleClaimTypes)
    {
        foreach (var claimType in possibleClaimTypes)
        {
            var claim = token.Claims.FirstOrDefault(c => c.Type == claimType);
            if (claim != null)
            {
                return claim.Value;
            }
        }
        return null;
    }

    private List<string> ExtractArrayClaim(JwtSecurityToken token, params string[] possibleClaimTypes)
    {
        var results = new List<string>();

        foreach (var claimType in possibleClaimTypes)
        {
            var claims = token.Claims.Where(c => c.Type == claimType).Select(c => c.Value);
            results.AddRange(claims);
        }

        // Check for serialized arrays (JSON arrays in single claims)
        foreach (var claimType in possibleClaimTypes)
        {
            var singleClaim = token.Claims.FirstOrDefault(c => c.Type == claimType);
            if (singleClaim != null && singleClaim.Value.StartsWith("[") && singleClaim.Value.EndsWith("]"))
            {
                try
                {
                    // Simple JSON array parsing
                    var arrayValue = singleClaim.Value.Trim('[', ']');
                    var items = arrayValue.Split(',')
                        .Select(s => s.Trim(' ', '\"', '\''))
                        .Where(s => !string.IsNullOrEmpty(s));
                    results.AddRange(items);
                }
                catch
                {
                    // Ignore if parsing fails
                }
            }
        }

        return results.Distinct().ToList();
    }

    private void UpdateNavigationBasedOnPermissions()
    {
        // Clear navigation items except the first one (Artikel)
        while (NavigationItems.Count > 1)
        {
            NavigationItems.RemoveAt(NavigationItems.Count - 1);
        }

        // Add navigation items based on permissions
        if (UserProfile.HasPermission("EMPLOYEE_VIEW") || UserProfile.IsInGroup("Mitarbeiter"))
        {
            NavigationItems.Add(new NavigationItemViewModel("Mitarbeiter",
                "M12 12C14.21 12 16 10.21 16 8C16 5.79 14.21 4 12 4C9.79 4 8 5.79 8 8C8 10.21 9.79 12 12 12ZM12 6C13.1 6 14 6.9 14 8C14 9.1 13.1 10 12 10C10.9 10 10 9.1 10 8C10 6.9 10.9 6 12 6ZM12 13C9.33 13 4 14.34 4 17V20H20V17C20 14.34 14.67 13 12 13ZM12 15C14.67 15 18 16.25 18 17V18H6V17C6 16.25 9.33 15 12 15Z",
                new RelayCommand(() => NavigateTo("Mitarbeiter", "Liste"))));
        }

        if (UserProfile.HasPermission("REPORT_VIEW"))
        {
            NavigationItems.Add(new NavigationItemViewModel("Berichte",
                "M19 3H5C3.9 3 3 3.9 3 5V19C3 20.1 3.9 21 5 21H19C20.1 21 21 20.1 21 19V5C21 3.9 20.1 3 19 3ZM19 19H5V5H19V19ZM7 10H9V17H7V10ZM11 7H13V17H11V7ZM15 13H17V17H15V13Z",
                new RelayCommand(() => NavigateTo("Berichte", "Zusammenfassung"))));
        }

        if (UserProfile.IsAdmin || UserProfile.IsInGroup("Administrator"))
        {
            NavigationItems.Add(new NavigationItemViewModel("Administration",
                "M19.43 12.98C19.47 12.66 19.5 12.34 19.5 12C19.5 11.66 19.47 11.34 19.43 11.02L21.54 9.37C21.73 9.22 21.78 8.95 21.66 8.73L19.66 5.27C19.54 5.05 19.27 4.97 19.05 5.05L16.56 6.05C16.04 5.65 15.48 5.32 14.87 5.07L14.5 2.42C14.46 2.18 14.25 2 14 2H10C9.75 2 9.54 2.18 9.5 2.42L9.13 5.07C8.52 5.32 7.96 5.66 7.44 6.05L4.95 5.05C4.72 4.96 4.46 5.05 4.34 5.27L2.34 8.73C2.21 8.95 2.27 9.22 2.46 9.37L4.57 11.02C4.53 11.34 4.5 11.67 4.5 12C4.5 12.33 4.53 12.66 4.57 12.98L2.46 14.63C2.27 14.78 2.22 15.05 2.34 15.27L4.34 18.73C4.46 18.95 4.73 19.03 4.95 18.95L7.44 17.95C7.96 18.35 8.52 18.68 9.13 18.93L9.5 21.58C9.54 21.82 9.75 22 10 22H14C14.25 22 14.46 21.82 14.5 21.58L14.87 18.93C15.48 18.68 16.04 18.34 16.56 17.95L19.05 18.95C19.28 19.04 19.54 18.95 19.66 18.73L21.66 15.27C21.78 15.05 21.73 14.78 21.54 14.63L19.43 12.98ZM12 15.5C10.07 15.5 8.5 13.93 8.5 12C8.5 10.07 10.07 8.5 12 8.5C13.93 8.5 15.5 10.07 15.5 12C15.5 13.93 13.93 15.5 12 15.5Z",
                new RelayCommand(() => NavigateTo("Administration", "Benutzer"))));
        }

        // You can add more permission-based navigation items here
    }

    // Für die NavigateTo-Methode müssen wir das RelayCommand-Attribut entfernen, 
    // da sie zwei Parameter hat und das nicht direkt unterstützt wird
    private void NavigateTo(string pageTitle, string pagePath)
    {
        // Update active navigation item
        foreach (var item in NavigationItems)
        {
            item.IsActive = item.Title == pageTitle;
        }

        // Update page title
        CurrentPageTitle = pageTitle;
        CurrentPagePath = pagePath;

        // Here you would update the current view
        // CurrentView = your view factory logic
    }

    // Einfache Methoden können das RelayCommand-Attribut weiterhin verwenden
    private void ExecuteSearch()
    {
        // Implement search functionality
    }

    private void OpenNotifications()
    {
        // Open notifications panel
    }

    private void OpenHelp()
    {
        // Open help documentation
    }
}