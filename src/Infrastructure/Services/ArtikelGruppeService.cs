using System;
using System.Linq.Expressions;
using Application.Interfaces.Services;
using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.Artikel.Request;
using Artikelsystem.Shared.DTOs.ArtikelGruppe.Request;
using AutoMapper;
using Domain.Common.BaseErrors;
using Domain.Common.ResultPattern;
using Domain.Entities.Artikel;
using Infrastructure.Common;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ArtikelGruppeService(AppDbContext context, IMapper mapper) : GenericRepository<Artikelgruppe>(context), IArtikelGruppeService
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Artikelgruppe.AnyAsync(a => a.Id == id);
    }

    public async Task<PagedResultDTO<GetAllArtikelGruppeResponse>> GetAllArtikelGruppen(int recordsPerPage, string? nameContains = null, int page = 1)
    {
        IQueryable<Artikelgruppe> query = _context.Artikelgruppe.Include(a => a.Artikel);

        if (!string.IsNullOrWhiteSpace(nameContains))
        {
            query = query.Where(a => a.Name.Contains(nameContains));
        }

        // Paging und Mapping anwenden
        return await PagingService.GetPagedAndMappedResultAsync<Artikelgruppe, GetAllArtikelGruppeResponse>(
            query,
            _mapper,
            page,
            recordsPerPage
        );
    }
}
