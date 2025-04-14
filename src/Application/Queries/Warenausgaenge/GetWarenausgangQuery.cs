using System;
using Application.Interfaces;
using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Filter;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;
using Domain.Common.BaseErrors;
using Domain.Common.ResultPattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Warenausgaenge;

public class GetWarenausgangQuery : IRequest<Result<PagedResultDTO<WarenausgangDto>>>
{
    public WarenausgangFilterDto Filter { get; set; } = new();
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}


public class GetWarenausgangQueryHandler : IRequestHandler<GetWarenausgangQuery, Result<PagedResultDTO<WarenausgangDto>>>
{
    private readonly IWarenausgangService _service;
    private readonly ILogger<GetWarenausgangQueryHandler> _logger;

    public GetWarenausgangQueryHandler(IWarenausgangService service, ILogger<GetWarenausgangQueryHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Result<PagedResultDTO<WarenausgangDto>>> Handle(GetWarenausgangQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetWarenausgaengeAsync wurde aufgerufen mit dem Filter: {@Filter}, PageNumber: {PageNumber}, PageSize: {PageSize}",
            request.Filter, request.PageNumber, request.PageSize);

        var result = await _service.GetWarenausgaengeAsync(request.Filter, request.PageNumber, request.PageSize);

        if (result.Items == null || !result.Items.Any())
        {
            return Result<PagedResultDTO<WarenausgangDto>>.Failure(BaseError.NotFound("Keine Warenausgänge gefunden", "Es wurden keine Warenausgänge gefunden."));
        }

        return Result<PagedResultDTO<WarenausgangDto>>.Success(result);
    }
}