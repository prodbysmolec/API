using System;
using Application.Interfaces.Services;
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

public class ArtikelGruppenQueryHandler(IArtikelGruppeService artikelGruppeService, IMapper mapper) : IRequestHandler<ArtikelGruppenQuery, Result<PagedResultDTO<GetAllArtikelGruppeResponse>>>
{
    private readonly IArtikelGruppeService _artikelGruppeService = artikelGruppeService;
    private readonly IMapper _mapper = mapper;
    public async Task<Result<PagedResultDTO<GetAllArtikelGruppeResponse>>> Handle(ArtikelGruppenQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _artikelGruppeService.GetAllArtikelGruppen(request.RecordsPerPage, request.NameContains, request.Page);
            
            if (result == null || !result.Items.Any())
            {
                return await Task.FromResult(Result<PagedResultDTO<GetAllArtikelGruppeResponse>>.Failure(BaseError.NotFound("Artikelgruppen nicht gefunden", "Keine Artikelgruppen gefunden.")));
            }

            return await Task.FromResult(Result<PagedResultDTO<GetAllArtikelGruppeResponse>>.Success(result));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(Result<PagedResultDTO<GetAllArtikelGruppeResponse>>.Failure(BaseError.InternalServerError("Fehler beim Abrufen", $"Fehler beim Abrufen der Artikelgruppen: {ex.Message}")));
        }
    }
}
