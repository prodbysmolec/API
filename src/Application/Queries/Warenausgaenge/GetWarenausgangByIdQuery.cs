using System;
using Application.Interfaces;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;
using Domain.Common.BaseErrors;
using Domain.Common.ResultPattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Warenausgaenge;

public class GetWarenausgangByIdQuery : IRequest<Result<WarenausgangDto>>
{
    public int Id { get; set; }

    public GetWarenausgangByIdQuery(int id)
    {
        Id = id;
    }
}


public class GetWarenausgangByIdQueryHandler : IRequestHandler<GetWarenausgangByIdQuery, Result<WarenausgangDto>>
{
    private readonly IWarenausgangService _service;
    private readonly ILogger<GetWarenausgangByIdQueryHandler> _logger;

    public GetWarenausgangByIdQueryHandler(IWarenausgangService service, ILogger<GetWarenausgangByIdQueryHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Result<WarenausgangDto>> Handle(GetWarenausgangByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetWarenausgangByIdAsync wurde aufgerufen mit der ID: {Id}", request.Id);

        var result = await _service.GetWarenausgangByIdAsync(request.Id);

        if (result == null)
        {
            return Result<WarenausgangDto>.Failure(BaseError.NotFound("Warenausgang nicht gefunden", $"Warenausgang mit der ID {request.Id} wurde nicht gefunden."));
        }

        return Result<WarenausgangDto>.Success(result);
    }
}
