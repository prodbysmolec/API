using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Client.Core.Helfer;
using Client.Core.Services.Auth;
using Client.Core.ViewModels.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Core.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase _currentMainView;
        
        [ObservableProperty]
        private UserProfileViewModel _userProfile;

        private readonly IServiceProvider _serviceProvider;
        private readonly ITokenService _tokenService;

        public MainWindowViewModel(IServiceProvider serviceProvider, ITokenService tokenService)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            
            // Benutzer-Profil initialisieren
            UserProfile = new UserProfileViewModel("Nicht angemeldet", "");
            
            // Login-View als Start anzeigen
            var loginViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
            loginViewModel.LoginSuccessful += OnLoginSuccessful;
            CurrentMainView = loginViewModel;
        }

        private async void OnLoginSuccessful(object sender, EventArgs e)
        {
            // Benutzerprofil aktualisieren
            await RefreshUserProfileAsync();
            
            // Zur Hauptansicht wechseln - hier müssen Sie Ihre MainView ViewModel-Klasse erstellen
            // Beispiel: CurrentMainView = _serviceProvider.GetRequiredService<DashboardViewModel>();
            CurrentMainView = _serviceProvider.GetRequiredService<MainViewModel>();
        }

        public async Task RefreshUserProfileAsync()
        {
            try
            {
                // Access Token abrufen
                var token = await _tokenService.GetAccessTokenAsync();
                
                if (string.IsNullOrEmpty(token))
                {
                    // Kein gültiges Token, Standardwerte verwenden
                    UserProfile.UpdateProfile("Nicht angemeldet", "", "", "", 0, new List<string>(), new List<string>());
                    return;
                }

                // JWT Token parsen
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                // Benutzerdaten aus Claims extrahieren
                var userId = ExtractClaimValue(jwtToken, "userId", "sub");
                var name = ExtractClaimValue(jwtToken, ClaimTypes.Name, "name", "given_name");
                var nachname = ExtractClaimValue(jwtToken, ClaimTypes.Surname, "family_name", "nachname");
                var email = ExtractClaimValue(jwtToken, ClaimTypes.Email, "email");
                var username = ExtractClaimValue(jwtToken, "preferred_username", "username");

                // Benutzergruppen extrahieren
                var userGruppen = ExtractArrayClaim(jwtToken, "userGruppen", "groups");
                
                // Berechtigungen extrahieren
                var permissions = ExtractArrayClaim(jwtToken, "permissions", "scope");

                // User ID-Konvertierung
                int parsedUserId = 0;
                int.TryParse(userId, out parsedUserId);

                // Benutzerprofil aktualisieren
                UserProfile.UpdateProfile(
                    name ?? "Unbekannt", 
                    nachname ?? "", 
                    email ?? "", 
                    username ?? "", 
                    parsedUserId,
                    userGruppen,
                    permissions);
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung
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

            // Prüfen auf serialisierte Arrays
            foreach (var claimType in possibleClaimTypes)
            {
                var singleClaim = token.Claims.FirstOrDefault(c => c.Type == claimType);
                if (singleClaim != null && singleClaim.Value.StartsWith("[") && singleClaim.Value.EndsWith("]"))
                {
                    try
                    {
                        var arrayValue = singleClaim.Value.Trim('[', ']');
                        var items = arrayValue.Split(',')
                            .Select(s => s.Trim(' ', '\"', '\''))
                            .Where(s => !string.IsNullOrEmpty(s));
                        results.AddRange(items);
                    }
                    catch
                    {
                        // Ignorieren, wenn das Parsing fehlschlägt
                    }
                }
            }

            return results.Distinct().ToList();
        }

        [RelayCommand]
        private void Logout()
        {
            // Token-Service Logout aufrufen
            _ = _tokenService.LogoutAsync();
            
            // Benutzer-Profil zurücksetzen
            UserProfile.UpdateProfile("Nicht angemeldet", "", "", "", 0, new List<string>(), new List<string>());
            
            // Zurück zur Login-View wechseln
            var loginViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
            loginViewModel.LoginSuccessful += OnLoginSuccessful;
            CurrentMainView = loginViewModel;
        }
    }
}