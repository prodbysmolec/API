// using System;
// using Application.Interfaces.Repositories;
// using Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Response;
// using Artikelsystem.Shared.Helfer;
// using AutoMapper;
// using Domain.Common.BaseErrors;
// using Domain.Common.ResultPattern;
// using MediatR;
// using Microsoft.AspNetCore.Http.HttpResults;
// using Microsoft.AspNetCore.Mvc;

// namespace Application.Queries.Wareneingaenge;

// public class GetWareneingaengeForArtikelHandler(IWareneingangRepository service) : IRequestHandler<GetWareneingaengeForArtikelQuery, Result<ListContainerDto<GetWareneingaengeForArtikelResponse>>>
// {
//     private readonly IWareneingangRepository _service = service;
//     public async Task<Result<ListContainerDto<GetWareneingaengeForArtikelResponse>>> Handle(GetWareneingaengeForArtikelQuery request, CancellationToken cancellationToken)
//     {
//         if(request._id <= 0)
//         {
//             return Result<ListContainerDto<GetWareneingaengeForArtikelResponse>>.Failure(BaseError.BadRequest("Id ist null oder negativ.", "Die Id des Artikels ist null oder negativ."));
//         }
//         var wareneingaenge = await _service.GetAllAsync();
//         if (!wareneingaenge.Any())
//         {
//             return Result<ListContainerDto<GetWareneingaengeForArtikelResponse>>.Failure(BaseError.NotFound("Keine Wareneingänge gefunden.", "Es wurden keine Wareneingänge für den Artikel gefunden."));
//         }

//         var container = new ListContainerDto<GetWareneingaengeForArtikelResponse>
//         {
//             Items = wareneingaenge
//         };

//         return Result<ListContainerDto<GetWareneingaengeForArtikelResponse>>.Success(container);
//     }
// }
