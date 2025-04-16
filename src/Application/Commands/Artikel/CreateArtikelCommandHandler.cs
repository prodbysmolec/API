using System;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Common.BaseErrors;
using Domain.Common.ResultPattern;
using Domain.Entities.Artikel;
using Domain.Errors;
using MediatR;

namespace Application.Commands.Artikel;

public class CreateArtikelCommandHandler(IArtikelRepository service, IMapper mapper) : IRequestHandler<CreateArtikelCommand, Result<bool>>
{
    private readonly IArtikelRepository _service = service;
    private readonly IMapper _mapper = mapper;
    public async Task<Result<bool>> Handle(CreateArtikelCommand request, CancellationToken cancellationToken)
    {
        try 
        {
            // 1. DTO -> Entity
            var artikel = _mapper.Map<Domain.Entities.Artikel.Artikel>(request);

            var response = await _service.AddArtikelAsync(artikel);

            // 4. Id zurückgeben
            return Result<bool>.Success(response);
        }
        catch
        {
            // Hier können Sie den Fehler protokollieren oder behandeln
            return Result<bool>.Failure(BaseError.InternalServerError("Artikel konnte nicht erstellt werden", "Das Erstellen des Artikels ist fehlgeschlagen."));
        }
    }
}
