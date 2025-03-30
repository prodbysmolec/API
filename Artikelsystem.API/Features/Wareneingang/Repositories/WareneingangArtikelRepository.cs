using Artikelsystem.Api.Features.Wareneingang.Models.Entitys;
using Artikelsystem.Api.Infrastructure.Persistence.Context;
using Artikelsystem.Api.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Artikelsystem.Api.Features.Wareneingang.Repositories;

public class WareneingangArtikelRepository : Repository<WareneingangArtikel>, IWareneingangArtikelRepository
{
    public WareneingangArtikelRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<WareneingangArtikel>> GetByWareneingangIdAsync(int wareneingangId)
    {
        return await _dbContext.WareneingangArtikel
            .Include(wa => wa.Artikel)
            .Where(wa => wa.WareneingangId == wareneingangId)
            .ToListAsync();
    }

    public async Task<IEnumerable<WareneingangArtikel>> GetByArtikelIdAsync(int artikelId)
    {
        return await _dbContext.WareneingangArtikel
            .Include(wa => wa.Wareneingang)
            .Where(wa => wa.ArtikelId == artikelId)
            .ToListAsync();
    }
}