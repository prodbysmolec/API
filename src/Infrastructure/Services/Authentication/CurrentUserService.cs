using System;
using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Authentication;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name;

    public IEnumerable<Claim> GetUserClaims()
    {
        return _httpContextAccessor.HttpContext?.User?.Claims ?? Enumerable.Empty<Claim>();
    }
}