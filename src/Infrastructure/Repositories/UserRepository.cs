using Application.Interfaces.Repositories;
using Domain.Common.BaseErrors;
using Domain.Common.ResultPattern;
using Domain.Entities.Authentication;
using Domain.Errors;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class UserRepository(AppDbContext context) : GenericRepository<User>(context), IUserRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Result<User>?> CreateAsync(User user)
    {
        // Check if the user already exists
        var existsResult = await ExistsByUsernameAsync(user.UserName);
        if (existsResult.IsSuccess && existsResult.Value)
        {
            return Result<User>.Failure(UserError.UserAlreadyExists(user.UserName));
        }
        var newUser = await base.AddAsync(user);
        var test = await _context.SaveChangesAsync();
        if(!newUser)
        {
            return Result<User>.Failure(UserError.UserCreationFailed(user.UserName));
        }
        return Result<User>.Success(user);
    }

    public async Task<Result<bool>> ExistsByEmailAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email);
    }

    public async Task<Result<bool>> ExistsByUsernameAsync(string username)
    {
        return await _context.Users
            .AnyAsync(u => u.UserName == username);
    }

    public async Task<Result<User>?> GetByIdAsync(int id)
    {
        var user = await _context.Users
            .Include(u => u.UserGruppenUsers)
            .ThenInclude(ug => ug.UserGruppen)
            .ThenInclude(gp => gp.GroupPermissions)
            .ThenInclude(gp => gp.Permission)
            .FirstOrDefaultAsync(u => u.Id == id);

        if(user == null)
        {
            return null!;
        }

        return user;
    }

    public async Task<Result<User>> GetByUserNameAsync(string username)
    {
        var user = await _context.Users
            .Include(u => u.UserGruppenUsers)
            .ThenInclude(ug => ug.UserGruppen)
            .ThenInclude(gp => gp.GroupPermissions)
            .ThenInclude(gp => gp.Permission)
            .FirstOrDefaultAsync(u => u.UserName == username);


        if(user == null)
        {
            return null!;
        }

        return user;
    }

    public async Task<Result<User>?> ValidateRefreshTokenAsync(int userId, string refreshToken)
    {
        var user = await _context.Users.FindAsync(userId);
        if(user is null || user.RefreshToken != refreshToken 
        || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return null;
        }
        return user;
    }
}
