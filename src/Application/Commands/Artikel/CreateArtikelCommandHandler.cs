using System;
using Application.Interfaces.Repositories;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Common.BaseErrors;
using Domain.Common.ResultPattern;
using Domain.Entities.Artikel;
using Domain.Errors;
using MediatR;

namespace Application.Commands.Artikel;

public class CreateArtikelCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateArtikelCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    public async Task<Result<bool>> Handle(CreateArtikelCommand request, CancellationToken cancellationToken)
    {
        try 
        {
            // 1. DTO -> Entity
            var artikel = _mapper.Map<Domain.Entities.Artikel.Artikel>(request);

            var response = await _unitOfWork.ArtikelRepository.AddArtikelAsync(artikel);
            await _unitOfWork.CommitAsync(cancellationToken);
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
