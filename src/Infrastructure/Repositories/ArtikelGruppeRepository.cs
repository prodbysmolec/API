using Application.Interfaces.Repositories;
using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.ArtikelGruppe.Request;
using AutoMapper;
using Domain.Entities.Artikel;
using Infrastructure.Common;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ArtikelGruppeRepository(AppDbContext context) : GenericRepository<Artikelgruppe>(context), IArtikelGruppeRepository
{
    private readonly AppDbContext _context = context;

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Artikelgruppe.AnyAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Artikelgruppe>> GetAllArtikelGruppen(string? nameContains = null)
    {
        IQueryable<Artikelgruppe> query = _context.Artikelgruppe.Include(a => a.Artikel);

        if (!string.IsNullOrWhiteSpace(nameContains))
        {
            query = query.Where(a => a.Name.Contains(nameContains));
        }

        return await query.ToListAsync();
    }
}
