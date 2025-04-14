using System;
using Application.Interfaces.Services;
using Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Request;
using Domain.Common.BaseErrors;
using Domain.Common.ResultPattern;
using MediatR;

namespace Application.Commands;

public class AddWareneingangsPositionCommand : IRequest<Result<int>>
{
    public AddWareneingangsPositionRequest Request { get; set; }

    public AddWareneingangsPositionCommand(AddWareneingangsPositionRequest request)
    {
        Request = request;
    }
}

public class AddWareneingangsPositionHandler : IRequestHandler<AddWareneingangsPositionCommand, Result<int>>
{
    private readonly IWareneingangService _wareneingangService;

    public AddWareneingangsPositionHandler(IWareneingangService wareneingangService)
    {
        _wareneingangService = wareneingangService;
    }

    public async Task<Result<int>> Handle(AddWareneingangsPositionCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _wareneingangService.AddWareneingangsPositionAsync(command.Request);
            return Result<int>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure(BaseError.InternalServerError("Fehler beim Hinzufügen der Wareneingangsposition", ex.Message));
        }
    }
}