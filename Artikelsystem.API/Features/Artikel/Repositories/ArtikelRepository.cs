using Artikelsystem.Api.Features.Artikel.Models.Entitys;
using Artikelsystem.Api.Infrastructure.Persistence.Context;
using Artikelsystem.Api.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Artikelsystem.Api.Features.Artikel.Repositories;

public class ArtikelRepository : Repository<Models.Entitys.Artikel>, IArtikelRepository
{
    public ArtikelRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Models.Entitys.Artikel?> GetArtikelMitStatistikAsync(int id)
    {
        return await _dbContext.Artikel
            .Include(a => a.ArtikelStatistik)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Models.Entitys.Artikel>> GetArtikelMitBestandUnterMindestbestandAsync()
    {
        return await _dbContext.Artikel
            .Where(a => a.Menge < a.Mindestbestand)
            .ToListAsync();
    }

    public async Task<bool> UpdateArtikelBestandAsync(int artikelId, int menge)
    {
        var artikel = await _dbContext.Artikel.FindAsync(artikelId);
        if (artikel == null)
        {
            return false;
        }

        artikel.Menge += menge;
        return true;
    }
}