using Client.Core.Services.Auth;
using Client.Core.ViewModels.Navigation;

namespace Client.Core.Helfer;

public class PermissionHelper
{
    private readonly ITokenService _tokenService;
    private readonly UserProfileViewModel _userProfile;

    public PermissionHelper(ITokenService tokenService, UserProfileViewModel userProfile)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _userProfile = userProfile ?? throw new ArgumentNullException(nameof(userProfile));
    }

    public bool HasPermission(string permissionCode)
    {
        return _userProfile.HasPermission(permissionCode);
    }
        
    public bool IsInGroup(string groupName)
    {
        return _userProfile.IsInGroup(groupName);
    }
        
    // Prüft, ob der Benutzer eine der angegebenen Berechtigungen hat
    public bool HasAnyPermission(params string[] permissionCodes)
    {
        foreach (var code in permissionCodes)
        {
            if (HasPermission(code))
                return true;
        }
        return false;
    }
        
    // Prüft, ob der Benutzer alle angegebenen Berechtigungen hat
    public bool HasAllPermissions(params string[] permissionCodes)
    {
        foreach (var code in permissionCodes)
        {
            if (!HasPermission(code))
                return false;
        }
        return true;
    }
}