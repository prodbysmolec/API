using Artikelsystem.Api.Features.Wareneingang.Models.Entitys;
using Artikelsystem.Api.Infrastructure.Persistence.Context;
using Artikelsystem.Api.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Artikelsystem.Api.Features.Wareneingang.Repositories;

public class WareneingangRepository : Repository<Models.Entitys.Wareneingang>, IWareneingangRepository
{
    public WareneingangRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Models.Entitys.Wareneingang?> GetWareneingangMitArtikelAsync(int id)
    {
        return await _dbContext.Wareneingaenge
            .Include(w => w.Lieferant)
            .Include(w => w.ArtikelPositionen)
                .ThenInclude(ap => ap.Artikel)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<IEnumerable<Models.Entitys.Wareneingang>> GetWareneingaengeByLieferantAsync(int lieferantId)
    {
        return await _dbContext.Wareneingaenge
            .Include(w => w.ArtikelPositionen)
                .ThenInclude(ap => ap.Artikel)
            .Where(w => w.LieferantId == lieferantId)
            .ToListAsync();
    }
}