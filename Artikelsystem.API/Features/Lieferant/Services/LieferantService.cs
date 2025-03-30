
using Artikelsystem.Api.Features.Lieferant.Models.DTOs;
using Artikelsystem.Api.Features.Lieferant.Models.Entitys;
using Artikelsystem.Api.Features.Lieferant.Services;
using Artikelsystem.Api.Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;

public class LieferantService : ILieferantService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LieferantService> _logger;

    public LieferantService(IUnitOfWork unitOfWork, ILogger<LieferantService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<LieferantDto>> GetAllLieferantenAsync(int page = 1, int recordsPerPage = 100, string? firmaContains = null, string? nameContains = null)
    {
        var lieferanten = await _unitOfWork.LieferantRepository.GetAllAsync();
        return lieferanten.Select(LieferantToLieferantDto);
    }

    public async Task<LieferantDto?> GetLieferantByIdAsync(int id)
    {
        var lieferant = await _unitOfWork.LieferantRepository.GetByIdAsync(id);
        return lieferant != null ? LieferantToLieferantDto(lieferant) : null;
    }

    public async Task<LieferantDto> CreateLieferantAsync(CreateLieferantRequest request)
    {
        var lieferant = new Lieferant
        {
            Firma = request.Firma,
            Name = request.Name,
            Vorname = request.Vorname,
            EmailAdresse = request.EmailAdresse,
            Strasse = request.Strasse,
            Hausnummer = request.Hausnummer,
            PLZ = request.PLZ,
            Ort = request.Ort,
            Telefonnummer = request.Telefonnummer,
            Notizen = request.Notizen
        };

        await _unitOfWork.LieferantRepository.AddAsync(lieferant);
        await _unitOfWork.SaveChangesAsync();

        return LieferantToLieferantDto(lieferant);
    }

    public async Task<LieferantDto?> UpdateLieferantAsync(int id, UpdateLieferantRequest request)
    {
        var lieferant = await _unitOfWork.LieferantRepository.GetByIdAsync(id);
        if (lieferant == null)
        {
            return null;
        }

        // Update nur die Properties, die nicht null sind
        if (request.Firma != null) lieferant.Firma = request.Firma;
        if (request.Name != null) lieferant.Name = request.Name;
        if (request.Vorname != null) lieferant.Vorname = request.Vorname;
        if (request.EmailAdresse != null) lieferant.EmailAdresse = request.EmailAdresse;
        if (request.Strasse != null) lieferant.Strasse = request.Strasse;
        if (request.Hausnummer != null) lieferant.Hausnummer = request.Hausnummer;
        if (request.PLZ != null) lieferant.PLZ = request.PLZ;
        if (request.Ort != null) lieferant.Ort = request.Ort;
        if (request.Telefonnummer != null) lieferant.Telefonnummer = request.Telefonnummer;

        // Notizen können null sein, also explizit setzen (auch wenn null)
        lieferant.Notizen = request.Notizen;

        _unitOfWork.LieferantRepository.Update(lieferant);
        await _unitOfWork.SaveChangesAsync();

        return LieferantToLieferantDto(lieferant);
    }

    public async Task<bool> DeleteLieferantAsync(int id)
    {
        var lieferant = await _unitOfWork.LieferantRepository.GetByIdAsync(id);
        if(lieferant == null) return false;

        _unitOfWork.LieferantRepository.Delete(lieferant);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private static LieferantDto LieferantToLieferantDto(Lieferant lieferant)
    {
        return new LieferantDto
        {
            Id = lieferant.Id,
            Firma = lieferant.Firma,
            Name = lieferant.Name,
            Vorname = lieferant.Vorname,
            EmailAdresse = lieferant.EmailAdresse,
            Strasse = lieferant.Strasse,
            Hausnummer = lieferant.Hausnummer,
            PLZ = lieferant.PLZ,
            Ort = lieferant.Ort,
            Telefonnummer = lieferant.Telefonnummer,
            Notizen = lieferant.Notizen
        };
    }
}