using System;
using Application.Interfaces;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Request;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;
using Domain.Common.BaseErrors;
using Domain.Common.ResultPattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Warenausgang;

public class CreateWarenausgangCommand : IRequest<Result<WarenausgangDto>>
{
    public WarenausgangRequestDto Dto { get; }

    public CreateWarenausgangCommand(WarenausgangRequestDto dto)
    {
        Dto = dto;
    }
}

public class CreateWarenausgangCommandHandler : IRequestHandler<CreateWarenausgangCommand, Result<WarenausgangDto>>
{
    private readonly IWarenausgangService _service;
    private readonly ILogger<CreateWarenausgangCommandHandler> _logger;

    public CreateWarenausgangCommandHandler(IWarenausgangService service, ILogger<CreateWarenausgangCommandHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Result<WarenausgangDto>> Handle(CreateWarenausgangCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var warenausgang = await _service.CreateWarenausgangAsync(request.Dto);

            if (warenausgang == null)
            {
                return Result<WarenausgangDto>.Failure(BaseError.BadRequest("Warenausgang konnte nicht erstellt werden", "Die Eingabedaten sind ungültig."));
            }

            return Result<WarenausgangDto>.Success(warenausgang);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Fehler bei der Erstellung des Warenausgangs: {Message}", ex.Message);
            return Result<WarenausgangDto>.Failure(BaseError.InternalServerError("Fehler bei der Erstellung", $"Fehler bei der Erstellung des Warenausgangs: {ex.Message}"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Fehler bei der Erstellung des Warenausgangs: {Message}", ex.Message);
            return Result<WarenausgangDto>.Failure(BaseError.BadRequest("Fehler bei der Erstellung", $"Fehler bei der Erstellung des Warenausgangs: {ex.Message}"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unbekannter Fehler bei der Erstellung des Warenausgangs: {Message}", ex.Message);
            return Result<WarenausgangDto>.Failure(BaseError.InternalServerError("Unbekannter Fehler", $"Unbekannter Fehler bei der Erstellung des Warenausgangs: {ex.Message}"));
        }
    }
}
