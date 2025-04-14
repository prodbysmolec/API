using System;
using Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Response;
using Artikelsystem.Shared.Helfer;
using Domain.Common.ResultPattern;
using MediatR;

namespace Application.Queries.Wareneingaenge;

public class GetWareneingaengeForArtikelQuery(int Id) : IRequest<Result<ListContainerDto<GetWareneingaengeForArtikelResponse>>>
{
    public readonly int _id = Id;
}
