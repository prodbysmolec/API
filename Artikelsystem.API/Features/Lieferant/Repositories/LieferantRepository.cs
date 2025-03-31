using Artikelsystem.Api.Features.Lieferant.Models.Entitys;
using Artikelsystem.Api.Infrastructure.Persistence.Context;
using Artikelsystem.Api.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Artikelsystem.Api.Features.Lieferant.Repositories;

public class LieferantRepository : Repository<Models.Entitys.Lieferant>, ILieferantRepository
{
    public LieferantRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Models.Entitys.Lieferant?> GetLieferantMitWareneingaengenAsync(int id)
    {
        return await _dbContext.Lieferanten
            .FirstOrDefaultAsync(l => l.Id == id);
    }
}