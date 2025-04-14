using System;
using Application.Interfaces.Services;
using Artikelsystem.Shared.DTOs.Artikel.Request;
using Artikelsystem.Shared.Helfer;
using AutoMapper;
using Domain.Common.BaseErrors;
using Domain.Common.ResultPattern;
using Domain.Errors;
using MediatR;

namespace Application.Queries.Artikel;

public class ArtikelQueryHandler(IArtikelService service, IMapper mapper) : IRequestHandler<GetArtikelQuery, Result<ListContainerDto<GetArtikelResponse>>>
{
    private readonly IArtikelService _service = service;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<ListContainerDto<GetArtikelResponse>>> Handle(GetArtikelQuery request, CancellationToken cancellationToken)
    {
        try 
        {
            // 1. Alle Artikel mit Paging und optionalen Filtern aus der Datenbank laden
            var pagedResult = await _service.GetAllArtikelAsync(request.request);

            // 2. Prüfen, ob Ergebnisse vorhanden sind
            if(pagedResult.Items == null || !pagedResult.Items.Any())
            {
                return await Task.FromResult(Result<ListContainerDto<GetArtikelResponse>>.Failure(ArtikelErrors.ArtikelNotFound()));
            }

            // 3. Artikel in Dto umwandeln
            var artikelDtos = _mapper.Map<List<GetArtikelResponse>>(pagedResult.Items);

            // 4. PagedResultDto in ArtikelListContainerDto umwandeln
            var employeeListContainer = new ListContainerDto<GetArtikelResponse>
            {
                Items = artikelDtos,
                Page = pagedResult.Page,
                RecordsPerPage = pagedResult.RecordsPerPage,
                TotalRecords = pagedResult.TotalRecords,
                TotalPages = pagedResult.TotalPages
            };

            // 5. Dto zurückgeben
            return Result<ListContainerDto<GetArtikelResponse>>.Success(employeeListContainer);
        }
        catch
        {
            return Result<ListContainerDto<GetArtikelResponse>>.Failure(BaseError.BadRequest("Mapping fehlgeschlagen", "Das Mapping der Artikel ist fehlgeschlagen."));
        }
    }
}
