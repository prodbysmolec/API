using Artikelsystem.Api.Features.Artikel.Models.Entitys;
using Artikelsystem.Api.Infrastructure.Persistence.Context;
using Artikelsystem.Api.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Artikelsystem.Api.Features.Artikel.Repositories;

public class ArtikelStatistikRepository : Repository<ArtikelStatistik>, IArtikelStatistikRepository
{
    public ArtikelStatistikRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> AktualisiereStatistikNachWareneingangAsync(int artikelId)
    {
        var artikel = await _dbContext.Artikel
            .Include(a => a.ArtikelStatistik)
            .Include(a => a.Wareneingaenge)
            .FirstOrDefaultAsync(a => a.Id == artikelId);

        if (artikel == null)
        {
            return false;
        }

        // Statistik erstellen, falls noch nicht vorhanden
        if (artikel.ArtikelStatistik == null)
        {
            artikel.ArtikelStatistik = new ArtikelStatistik
            {
                ArtikelId = artikelId
            };
        }

        // Menge und Durchschnittspreise berechnen
        var wareneingaenge = await _dbContext.WareneingangArtikel
            .Where(w => w.ArtikelId == artikelId)
            .ToListAsync();

        if (wareneingaenge.Any())
        {
            var gesamtMenge = wareneingaenge.Sum(w => w.Menge);
            var gesamtWert = wareneingaenge.Sum(w => w.Menge * w.Einzelpreis);

            artikel.ArtikelStatistik.Gesamtmenge = gesamtMenge;
            artikel.ArtikelStatistik.DurchschnittlicherEinzelpreis = gesamtMenge > 0 
                ? gesamtWert / gesamtMenge 
                : 0;
        }
        else
        {
            artikel.ArtikelStatistik.Gesamtmenge = 0;
            artikel.ArtikelStatistik.DurchschnittlicherEinzelpreis = 0;
        }

        return true;
    }
}