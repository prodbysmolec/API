using Domain.Entities.Authentication;

namespace Application.Interfaces;

public interface IJwtTokenGenerator
{
    public Task<string> CreateAccessTokenAsync(User user);
    Task<string> GenerateAndSaveRefreshTokenAsync(User user);
}
