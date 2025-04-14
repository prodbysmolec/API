using System;
using Application.Interfaces.Services;
using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Response;
using Domain.Common.BaseErrors;
using Domain.Common.ResultPattern;
using MediatR;

namespace Application.Queries.Wareneingaenge;

public class GetAlleWareneingaengeQuery: IRequest<Result<PagedResultDTO<GetAlleWareneingaengeResponse>>> 
{
    public int Page { get; set; }
    public int RecordsPerPage { get; set; }

    public GetAlleWareneingaengeQuery(int page, int recordsPerPage)
    {
        Page = page;
        RecordsPerPage = recordsPerPage;
    }
}

public class GetAlleWareneingaengeHandler : IRequestHandler<GetAlleWareneingaengeQuery, Result<PagedResultDTO<GetAlleWareneingaengeResponse>>>
{
    private readonly IWareneingangService _wareneingangService;

    public GetAlleWareneingaengeHandler(IWareneingangService wareneingangService)
    {
        _wareneingangService = wareneingangService;
    }

    public async Task<Result<PagedResultDTO<GetAlleWareneingaengeResponse>>> Handle(GetAlleWareneingaengeQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _wareneingangService.GetAlleWareneingaengeAsync(query.Page, query.RecordsPerPage);
            if(result == null || result.Items.Count == 0)
            {
                return Result<PagedResultDTO<GetAlleWareneingaengeResponse>>.Failure(BaseError.NotFound("Wareneingänge nicht gefunden", "Keine Wareneingänge gefunden."));
            }
            return Result<PagedResultDTO<GetAlleWareneingaengeResponse>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<PagedResultDTO<GetAlleWareneingaengeResponse>>.Failure(BaseError.InternalServerError("Fehler beim Abrufen", $"Fehler beim Abrufen der Wareneingänge: {ex.Message}"));
        }
    }
}
