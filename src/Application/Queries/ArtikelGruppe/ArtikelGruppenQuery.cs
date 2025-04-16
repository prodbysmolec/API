using System;
using Application.Interfaces.Repositories;
using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.ArtikelGruppe.Request;
using Artikelsystem.Shared.Helfer;
using AutoMapper;
using Domain.Common.BaseErrors;
using Domain.Common.ResultPattern;
using MediatR;

namespace Application.Queries.ArtikelGruppe;

public class ArtikelGruppenQuery : IRequest<Result<PagedResultDTO<GetAllArtikelGruppeResponse>>>
{
    public ArtikelGruppenQuery(FilteringDTO query)
    {
        Page = query.Page;
        RecordsPerPage = query.RecordsPerPage;
        SortBy = query.SortBy;
        SortDesc = query.SortDesc;
        NameContains = query.NameContains;
    }

    public int Page { get; set; } = 1;
    public int RecordsPerPage { get; set; } = 10;
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; }
    public string? NameContains { get; set; }
}

public class ArtikelGruppenQueryHandler(IArtikelGruppeRepository ArtikelGruppeRepository, IMapper mapper) : IRequestHandler<ArtikelGruppenQuery, Result<PagedResultDTO<GetAllArtikelGruppeResponse>>>
{
    private readonly IArtikelGruppeRepository _ArtikelGruppeRepository = ArtikelGruppeRepository;
    private readonly IMapper _mapper = mapper;
    public async Task<Result<PagedResultDTO<GetAllArtikelGruppeResponse>>> Handle(ArtikelGruppenQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var artikelgruppen = await _ArtikelGruppeRepository.GetAllArtikelGruppen(request.NameContains);

            if (artikelgruppen == null || !artikelgruppen.Any())
            {
                return await Task.FromResult(Result<PagedResultDTO<GetAllArtikelGruppeResponse>>.Failure(BaseError.NotFound("Artikelgruppen nicht gefunden", "Keine Artikelgruppen gefunden.")));
            }

            int page = request.Page;
            int recordsPerPage = request.RecordsPerPage;
            var pagedArtikelGruppen = artikelgruppen
                .Skip((page - 1) * recordsPerPage)
                .Take(recordsPerPage)
                .ToList();

            // Mapping der Entitäten zu DTOs durchführen
            var mappedItems = _mapper.Map<List<GetAllArtikelGruppeResponse>>(pagedArtikelGruppen);

            // PagedResultDTO erstellen
            var pagedResult = new PagedResultDTO<GetAllArtikelGruppeResponse>
            {
                Items = mappedItems,
                Page = page,
                RecordsPerPage = recordsPerPage,
                TotalRecords = artikelgruppen.Count()
            };
            
            return await Task.FromResult(Result<PagedResultDTO<GetAllArtikelGruppeResponse>>.Success(pagedResult));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(Result<PagedResultDTO<GetAllArtikelGruppeResponse>>.Failure(BaseError.InternalServerError("Fehler beim Abrufen", $"Fehler beim Abrufen der Artikelgruppen: {ex.Message}")));
        }
    }
}
