using Application.Interfaces.Services;
using Artikelsystem.Shared.DTOs.Artikel.Request;
using AutoMapper;
using Domain.Common.BaseErrors;
using Domain.Common.ResultPattern;
using MediatR;

namespace Application.Queries.Artikel;

public class GetArtikelByIdQueryHandler(IArtikelService service, IMapper mapper) : IRequestHandler<GetArtikelByIdQuery, Result<GetArtikelResponse>>
{
    private readonly IArtikelService _service = service;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<GetArtikelResponse>> Handle(GetArtikelByIdQuery request, CancellationToken cancellationToken)
    {
        try 
        {
            if(request.Id <= 0)
            {
                return await Task.FromResult(Result<GetArtikelResponse>.Failure(BaseError.BadRequest("ID ist 0 oder negativ", "Die ID darf nicht 0 oder negativ sein. Eingegebene ID:" + request.Id.ToString())));
            }
            var artikel = await _service.GetArtikelByIdAsync(request.Id);

            if (artikel == null)
            {
                return await Task.FromResult(Result<GetArtikelResponse>.Failure(BaseError.NotFound("Artikel nicht gefunden", "Der Artikel mit der angegebenen ID wurde nicht gefunden. Eingegebene ID:" + request.Id.ToString())));
            }
            var artikelDto = _mapper.Map<GetArtikelResponse>(artikel);
            return await Task.FromResult(Result<GetArtikelResponse>.Success(artikelDto));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(Result<GetArtikelResponse>.Failure(BaseError.InternalServerError("Interner Serverfehler", "Ein interner Serverfehler ist aufgetreten. Fehlerdetails: " + ex.Message)));
        }
    }
}
