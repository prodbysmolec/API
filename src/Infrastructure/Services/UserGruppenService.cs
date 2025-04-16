using System;
using Application.Interfaces.Services;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserGruppenService(AppDbContext context) : IUserGruppenService
{
    private readonly AppDbContext _context = context;
    public async Task<IEnumerable<object>> GetUserGroupNamesAsync(int Id)
    {
        var UserGruppen = 
                await _context.UserGruppenUsers
                .Include(ugu => ugu.UserGruppen)
                .Where(ugu => ugu.UserID == Id)
                .Select(ugu => ugu.UserGruppen.Name)
                .ToListAsync();

        return UserGruppen;
    }
}
