using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Artikelsystem.Api.Features.Inventur.Models.Dtos;

namespace Artikelsystem.Api.Features.Inventur.Services;

public interface IInventurService
{
    Task<InventurDto> ErstelleInventur(CreateInventurRequest request);
    Task<InventurDto> StarteInventur(int inventurId);
    Task<InventurDto> GetInventurById(int inventurId);
    Task<List<InventurDto>> GetAlleInventuren();
    Task<InventurPositionDto> AktualisieereInventurPosition(UpdateInventurPositionRequest request);
    Task<InventurDto> SchliesseInventurAb(int inventurId);
    Task<List<InventurBerichtDto>> GetInventurBerichte();
    Task<InventurBerichtDto> GetInventurBerichtById(int berichtId);
    Task<InventurBerichtDto?> GetInventurBerichtFuerInventur(int inventurId);
    Task<InventurBerichtDto> GenerateInventurBericht(int inventurId, string benutzer);
}