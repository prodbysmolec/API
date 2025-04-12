using Domain.Entities.Authentication;

namespace Application.Interfaces;

public interface IJwtTokenGenerator
{
    public Task<string> CreateTokenAsync(User user);
}
