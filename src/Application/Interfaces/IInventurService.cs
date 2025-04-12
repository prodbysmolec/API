using Artikelsystem.Shared.DTOs.Inventur;

namespace Application.Interfaces;

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
    Task<InventurDto> DeleteInventur(int inventurId);
}