using System;
using Application.Interfaces.Repositories;
using Domain.Common.ResultPattern;
using Domain.Entities.Artikel;
using Domain.Entities.Authentication;

namespace Application.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<Result<User>?> CreateAsync(User user);
    Task<Result<bool>> ExistsByUsernameAsync(string username);
    Task<Result<bool>> ExistsByEmailAsync(string email);
    Task<Result<User>?> GetByIdAsync(int id);
    Task<Result<User>> GetByUserNameAsync(string username);
    Task<Result<User>?> ValidateRefreshTokenAsync(int userId, string refreshToken);
}
