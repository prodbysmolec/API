using Application.Interfaces.Repositories;
using Domain.Entities.Authentication;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PermissionRepository(AppDbContext context) : GenericRepository<Permission>(context), IPermissionRepository
{
    private readonly AppDbContext _context = context;
    public async Task<List<string>> GetUserPermissionCodesAsync(int userId)
    {
        // alle PermissionCodes, die mit den Gruppen des Users verknüpft sind
        var permissionCodes = await _context.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.UserGruppenUsers)
            .Select(ugu => ugu.UserGruppen)
            .SelectMany(ug => ug.GroupPermissions)
            .Select(gp => gp.Permission.Code)
            .Distinct()
            .ToListAsync();
            
        return permissionCodes;
    }
}
