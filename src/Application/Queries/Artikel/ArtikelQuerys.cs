using System;
using Artikelsystem.Shared.DTOs.Artikel.Request;
using Artikelsystem.Shared.Helfer;
using Domain.Common.ResultPattern;
using MediatR;

namespace Application.Queries.Artikel;

public record GetArtikelQuery(GetAllArtikelRequest request) : IRequest<Result<ListContainerDto<GetArtikelResponse>>>;

public record GetArtikelByIdQuery(int Id) : IRequest<Result<GetArtikelResponse>>;
