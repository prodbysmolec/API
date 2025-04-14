using System;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Common.BaseErrors;
using Domain.Common.ResultPattern;
using Domain.Entities.Artikel;
using Domain.Errors;
using MediatR;

namespace Application.Commands.Artikel;

public class CreateArtikelCommandHandler(IArtikelService service, IMapper mapper) : IRequestHandler<CreateArtikelCommand, Result<int>>
{
    private readonly IArtikelService _service = service;
    private readonly IMapper _mapper = mapper;
    public async Task<Result<int>> Handle(CreateArtikelCommand request, CancellationToken cancellationToken)
    {
        try 
        {
            // 1. DTO -> Entity
            var artikel = _mapper.Map<Domain.Entities.Artikel.Artikel>(request);

            var response = await _service.AddArtikelAsync(artikel);

            // 4. Id zurückgeben
            return Result<int>.Success(response);
        }
        catch
        {
            // Hier können Sie den Fehler protokollieren oder behandeln
            return Result<int>.Failure(BaseError.InternalServerError("Artikel konnte nicht erstellt werden", "Das Erstellen des Artikels ist fehlgeschlagen."));
        }
    }
}
