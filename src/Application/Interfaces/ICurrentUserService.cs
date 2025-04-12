using System.Security.Claims;

namespace Application.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    IEnumerable<Claim> GetUserClaims();
}
