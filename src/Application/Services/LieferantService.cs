// using Artikelsystem.Shared.DTOs.Lieferant;
// using Domain.Entities.Lieferant;
// using API.Infrastructure.Persistence.Context;
// using Artikelsystem.Shared.DTOs.Lieferant.Request;
// using Microsoft.AspNetCore.Http.HttpResults;
// using Microsoft.EntityFrameworkCore;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace Application.Interfaces
// {

//     public class LieferantService : ILieferantService
//     {
//         private readonly AppDbContext _context;

//         public LieferantService(AppDbContext context)
//         {
//             _context = context;
//         }

//         /// <summary>
//         /// Holt alle Lieferanten aus der Datenbank
//         /// </summary>
//         public async Task<List<LieferantDto>> GetAllLieferanten(bool nurAktive = false, bool alles = false)
//         {
//             var query = _context.Lieferanten.AsQueryable();

//             if (nurAktive)
//             {
//                 query = query.Where(l => l.IstAktiv);
//             }

//             if (alles)
//             {
//                 query.Include(l => l.ArtikelLieferanten)
//                     .ThenInclude(l => l.Artikel);
//             }


//             var lieferanten = await query
//                 .OrderBy(l => l.Firma)
//                 .ThenBy(l => l.Name)
//                 .ToListAsync();

//             return lieferanten.Select(MapToDto).ToList();
//         }

//         /// <summary>
//         /// Holt einen Lieferanten anhand seiner ID
//         /// </summary>
//         public async Task<LieferantDetailDto?> GetLieferantById(bool? alles, int id)
//         {
//             var query = _context.Lieferanten.AsQueryable();

//             if(alles == true)
//             {
//                 query.Include(l => l.ArtikelLieferanten)
//                     .ThenInclude(l => l.Artikel);
//             }

//             var lieferant = await query.FirstOrDefaultAsync(l => l.Id == id);

//             if(lieferant == null)
//             {
//                 return null;
//             }

//             return MapToDetailDto(lieferant);
//         }

//         /// <summary>
//         /// Erstellt einen neuen Lieferanten
//         /// </summary>
//         public async Task<LieferantDto> ErstelleLieferant(CreateLieferantRequest request)
//         {
//             var lieferant = new Domain.Entities.Lieferant.Lieferant
//             {
//                 Firma = request.Firma,
//                 Name = request.Name,
//                 Vorname = request.Vorname,
//                 EmailAdresse = request.EmailAdresse,
//                 Strasse = request.Strasse,
//                 Hausnummer = request.Hausnummer,
//                 PLZ = request.PLZ,
//                 Ort = request.Ort,
//                 Telefonnummer = request.Telefonnummer,
//                 Notizen = request.Notizen,
//                 IstAktiv = true
//             };

//             _context.Lieferanten.Add(lieferant);
//             await _context.SaveChangesAsync();

//             return MapToDto(lieferant);
//         }

//         /// <summary>
//         /// Aktualisiert einen bestehenden Lieferanten
//         /// </summary>
//         public async Task<LieferantDto?> UpdateLieferantAsync(int id, UpdateLieferantRequest request)
//         {
//             var lieferant = await _context.Lieferanten.FindAsync(id);

//             if (lieferant == null)
//             {
//                 return null;
//             }

//             // Eigenschaften aktualisieren
//             lieferant.Firma = request.Firma;
//             lieferant.Name = request.Name;
//             lieferant.Vorname = request.Vorname;
//             lieferant.EmailAdresse = request.EmailAdresse;
//             lieferant.Strasse = request.Strasse;
//             lieferant.Hausnummer = request.Hausnummer;
//             lieferant.PLZ = request.PLZ;
//             lieferant.Ort = request.Ort;
//             lieferant.Telefonnummer = request.Telefonnummer;
//             lieferant.Notizen = request.Notizen;
//             lieferant.IstAktiv = request.IstAktiv;

//             _context.Entry(lieferant).State = EntityState.Modified;
//             await _context.SaveChangesAsync();

//             return MapToDto(lieferant);
//         }

//         /// <summary>
//         /// Markiert einen Lieferanten als inaktiv (soft delete)
//         /// </summary>
//         public async Task<bool> DeactivateLieferantAsync(int id)
//         {
//             var lieferant = await _context.Lieferanten.FindAsync(id);

//             if (lieferant == null)
//             {
//                 return false;
//             }

//             lieferant.IstAktiv = false;

//             _context.Entry(lieferant).State = EntityState.Modified;
//             await _context.SaveChangesAsync();

//             return true;
//         }

//         /// <summary>
//         /// Löscht einen Lieferanten und alle damit verbundenen Daten
//         /// </summary>
//         public async Task<bool> DeleteLieferantAsync(int id)
//         {
//             var lieferant = await _context.Lieferanten.FindAsync(id);
//             if (lieferant == null)
//             {
//                 throw new NullReferenceException($"Lieferant mit ID {id} nicht gefunden.");
//             }

//             _context.Lieferanten.Remove(lieferant);
//             await _context.SaveChangesAsync();

//             return true;
//         }

//         /// <summary>
//         /// Sucht nach Lieferanten anhand verschiedener Kriterien
//         /// </summary>
//         public async Task<List<LieferantDto>> SearchLieferantenAsync(string suchbegriff)
//         {
//             if (string.IsNullOrEmpty(suchbegriff))
//             {
//                 return await GetAllLieferanten();
//             }

//             suchbegriff = suchbegriff.ToLower();

//             var lieferanten = await _context.Lieferanten
//                 .Where(l =>
//                     l.Firma.ToLower().Contains(suchbegriff) ||
//                     l.Name.ToLower().Contains(suchbegriff) ||
//                     l.Vorname.ToLower().Contains(suchbegriff) ||
//                     l.Ort.ToLower().Contains(suchbegriff) ||
//                     l.EmailAdresse.ToLower().Contains(suchbegriff)
//                 )
//                 .OrderBy(l => l.Firma)
//                 .ThenBy(l => l.Name)
//                 .ToListAsync();

//             return lieferanten.Select(MapToDto).ToList();
//         }

//         #region Helper Methods

//         private LieferantDto MapToDto(Domain.Entities.Lieferant.Lieferant lieferant)
//         {
//             return new LieferantDto
//             {
//                 Id = lieferant.Id,
//                 Firma = lieferant.Firma,
//                 Name = lieferant.Name,
//                 Vorname = lieferant.Vorname,
//                 EmailAdresse = lieferant.EmailAdresse,
//                 Strasse = lieferant.Strasse,
//                 Hausnummer = lieferant.Hausnummer,
//                 PLZ = lieferant.PLZ,
//                 Ort = lieferant.Ort,
//                 Telefonnummer = lieferant.Telefonnummer,
//                 Notizen = lieferant.Notizen,
//                 IstAktiv = lieferant.IstAktiv,
//                 AdresseFormatiert = $"{lieferant.Strasse} {lieferant.Hausnummer}, {lieferant.PLZ} {lieferant.Ort}"
//             };
//         }

//         private LieferantDetailDto MapToDetailDto(Domain.Entities.Lieferant.Lieferant lieferant)
//         {
//             return new LieferantDetailDto
//             {
//                 Id = lieferant.Id,
//                 Firma = lieferant.Firma,
//                 Name = lieferant.Name,
//                 Vorname = lieferant.Vorname,
//                 EmailAdresse = lieferant.EmailAdresse,
//                 Strasse = lieferant.Strasse,
//                 Hausnummer = lieferant.Hausnummer,
//                 PLZ = lieferant.PLZ,
//                 Ort = lieferant.Ort,
//                 Telefonnummer = lieferant.Telefonnummer,
//                 Notizen = lieferant.Notizen,
//                 IstAktiv = lieferant.IstAktiv,
//                 AdresseFormatiert = $"{lieferant.Strasse} {lieferant.Hausnummer}, {lieferant.PLZ} {lieferant.Ort}",
//                 ArtikelAnzahl = lieferant.ArtikelLieferanten?.Count(al => al.IstAktiv) ?? 0,
//                 AktiveArtikel = lieferant.ArtikelLieferanten?
//                     .Where(al => al.IstAktiv)
//                     .Select(al => new LieferantArtikelDto
//                     {
//                         ArtikelId = al.ArtikelId,
//                         ArtikelName = al.Artikel?.Name ?? "Unbekannter Artikel",
//                         Einkaufspreis = al.Einkaufspreis,
//                         IstPrimaerLieferant = al.IstPrimaerLieferant,
//                         ArtikelNrBeimLieferanten = al.ArtikelNrBeimLieferanten,
//                         SeitDatum = al.GueltigVon
//                     })
//                     .ToList() ?? new List<LieferantArtikelDto>()
//             };
//         }

//         #endregion
//     }
// }