using Artikelsystem.Shared.DTOs.Lieferant;

namespace Application.Interfaces;

public interface IArtikelLieferantService
{
    public Task<List<ArtikelLieferantDto>> GetAllLieferantenForArtikelAsync(int artikelId, bool nurAktive = false);
    public Task<ArtikelLieferantDto?> GetPrimaryLieferantForArtikelAsync(int artikelId);
    public Task<ArtikelLieferantDto> AddLieferantToArtikelAsync(int artikelId, int lieferantId, ArtikelLieferantDto dto);
    public Task<ArtikelLieferantDto> ChangeLieferantAsync(int artikelId, int neuerLieferantId, ArtikelLieferantUpdateDto dto);
    public Task<ArtikelLieferantDto?> UpdateArtikelLieferantAsync(int artikelId, int lieferantId, ArtikelLieferantUpdateDto dto);  
    public Task<bool> DeactivateArtikelLieferantAsync(int artikelId, int lieferantId);
    public Task<bool> DeleteArtikelLieferantAsync(int artikelId, int lieferantId);
    public Task<List<ArtikelLieferantDto>> SearchLieferantenForArtikelAsync(int artikelId, string suchbegriff);
    public Task<List<ArtikelLieferantDto>> GetArtikelByLieferantAsync(int lieferantId, bool nurAktive = true);
}
