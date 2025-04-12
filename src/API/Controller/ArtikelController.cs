// using System.Runtime.CompilerServices;
// using API.Infrastructure.Persistence.Context;
// using API.Common.Controllers;
// using Artikelsystem.Shared.DTOs.Artikel.Request;
// using Artikelsystem.Shared.DTOs.Artikel.Response;
// using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;
// using Artikelsystem.Shared.DTOs.Wareneingang.Dtos.Response;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Domain.Entities.Artikel;

// namespace API.Controller;
// public class ArtikelController : BaseController
// {
//     private readonly ILogger<ArtikelController> _logger;
//     private readonly AppDbContext _dbContext;

//     public ArtikelController(
//         ILogger<ArtikelController> logger,
//         AppDbContext dbContext
//     )
//     {
//         _logger = logger;
//         _dbContext = dbContext;
//     }

//     /// <summary>
//     /// Gets all articles in the system with filtering options.
//     /// </summary>
//     /// <param name="request">Filter and pagination parameters</param>
//     /// <returns>Returns the filtered articles in a JSON array.</returns>
//     [HttpGet]
//     [ProducesResponseType(typeof(IEnumerable<GetArtikelResponse>), StatusCodes.Status200OK)]
//     [ProducesResponseType(typeof(IEnumerable<GetArtikelResponse>), StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> GetAllArtikel([FromQuery] GetAllArtikelRequest? request)
//     {
//         int page = request?.Page ?? 1;
//         int recordsPerPage = request?.RecordsPerPage ?? 100;

//         // Start with base query including related data
//         IQueryable<Domain.Entities.Artikel.Artikel> query = _dbContext.Artikel
//             .Include(a => a.ArtikelStatistik);

//         // Apply filters if request is not null
//         if (request != null)
//         {
//             ApplyFilters(ref query, request);
//             ApplySorting(ref query, request);
//         }
//         else
//         {
//             // Default sorting by ID
//             query = query.OrderBy(a => a.Id);
//         }

//         // Apply pagination after all filters
//         query = query.Skip((page - 1) * recordsPerPage).Take(recordsPerPage);

//         var artikel = await query.ToArrayAsync();

//         return Ok(artikel.Select(ArtikelToGetArtikelResponse));
//     }

//     /// <summary>
//     /// Gets an article by its ID with options to include related data.
//     /// </summary>
//     /// <param name="id">The ID of the article</param>
//     /// <param name="request">Options for including related data</param>
//     /// <returns>The article with requested related data</returns>
//     [HttpGet("{id:int}")]
//     [ProducesResponseType(typeof(GetArtikelResponse), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> GetArtikelById(int id, [FromQuery] GetArtikelByIdRequest? request = null)
//     {
//         _logger.LogInformation("Getting article with ID: {ArtikelId}", id);
//         request ??= new GetArtikelByIdRequest();

//         // Start with base query
//         IQueryable<Domain.Entities.Artikel.Artikel> query = _dbContext.Artikel;

//         // Include related data based on request
//         IncludeRelatedData(ref query, request);

//         var artikel = await query.SingleOrDefaultAsync(a => a.Id == id);

//         if (artikel == null)
//         {
//             _logger.LogWarning("Article with ID {ArtikelId} not found", id);
//             return NotFound();
//         }

//         var artikelResponse = ArtikelToGetArtikelResponse(artikel);
//         _logger.LogDebug("Successfully retrieved article with ID: {ArtikelId}", id);

//         return Ok(artikelResponse);
//     }

//     /// <summary>
//     /// Gets warehouse receipts for a specific article.
//     /// </summary>
//     /// <param name="artikelId">The ID of the article</param>
//     /// <returns>List of warehouse receipts for the article</returns>
//     [HttpGet("{artikelId:int}/wareneingaenge")]
//     [ProducesResponseType(typeof(IEnumerable<WareneingangArtikelPositionenDto>), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> GetWareneingaengeForArtikel(int artikelId)
//     {
//         var artikel = await _dbContext.Artikel
//             .Include(a => a.Wareneingaenge)
//             .ThenInclude(w => w.Wareneingang)
//             .SingleOrDefaultAsync(a => a.Id == artikelId);

//         if (artikel == null)
//         {
//             return NotFound();
//         }

//         var wareneingaenge = artikel.Wareneingaenge.Select(e => new WareneingangArtikelPositionenDto
//         {
//             Id = e.Id,
//             ArtikelId = e.ArtikelId,
//             WareneingangId = e.WareneingangId,
//             Menge = e.Menge,
//             Einzelpreis = e.Einzelpreis,
//             Gesamtpreis = e.Gesamtpreis,
//             Wareneingang = e.Wareneingang != null ? new WareneingangDto
//             {
//                 Id = e.Wareneingang.Id,
//                 AllgemeineBemerkungen = e.Wareneingang.AllgemeineBemerkungen ?? "",
//             } : null
//         });

//         return Ok(wareneingaenge);
//     }

//     /// <summary>
//     /// Gets warehouse issues for a specific article.
//     /// </summary>
//     /// <param name="artikelId">The ID of the article</param>
//     /// <returns>List of warehouse issues for the article</returns>
//     [HttpGet("{artikelId:int}/warenausgaenge")]
//     [ProducesResponseType(typeof(IEnumerable<WarenausgangArtikelPositionenDto>), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> GetWarenausgaengeForArtikel(int artikelId)
//     {
//         var artikel = await _dbContext.Artikel
//             .Include(a => a.Warenausgaenge)
//             .ThenInclude(w => w.Warenausgang)
//             .SingleOrDefaultAsync(a => a.Id == artikelId);

//         if (artikel == null)
//         {
//             return NotFound();
//         }

//         var warenausgaenge = artikel.Warenausgaenge.Select(w => new WarenausgangArtikelPositionenDto
//         {
//             Id = w.Id,
//             WarenausgangId = w.WarenausgangId,
//             ArtikelId = w.ArtikelId,
//             ArtikelName = w.Artikel?.Name ?? "",
//             Menge = w.Menge,
//             Bemerkung = w.Bemerkung ?? "",
//             Verkaufspreis = w.Verkaufspreis,
//             Rechnungsnummer = w.Rechnungsnummer ?? "",
//             Gesamtpreis = w.Gesamtpreis,
//             Warenausgang = w.Warenausgang != null ? new WarenausgangDto
//             {
//                 Id = w.Warenausgang.Id,
//                 AllgemeineBemerkungen = w.Warenausgang.AllgemeineBemerkungen ?? "",
//                 ErstelltAm = w.Warenausgang.ErstelltAm,
//                 BearbeitetAm = w.Warenausgang.BearbeitetAm,
//                 ErstelltVon = w.Warenausgang.ErstelltVon,
//                 BearbeitetVon = w.Warenausgang.BearbeitetVon,
//                 Zweck = w.Warenausgang.Zweck
//             } : null
//         });
//         return Ok(warenausgaenge);
//     }

//     /// <summary>
//     /// Gets statistics for a specific article.
//     /// </summary>
//     /// <param name="artikelId">The ID of the article</param>
//     /// <returns>Statistics for the specified article</returns>
//     [HttpGet("{artikelId:int}/statistik")]
//     [ProducesResponseType(typeof(ArtikelStatistikDto), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> GetArtikelStatistik(int artikelId)
//     {
//         var artikel = await _dbContext.Artikel
//             .Include(a => a.ArtikelStatistik)
//             .SingleOrDefaultAsync(a => a.Id == artikelId);

//         if (artikel == null)
//         {
//             return NotFound();
//         }

//         if (artikel.ArtikelStatistik == null)
//         {
//             return NotFound("No statistics available for this article");
//         }

//         var statistik = new ArtikelStatistikDto
//         {
//             Gesamtmenge = artikel.ArtikelStatistik.Gesamtmenge,
//             DurchschnittlicherEinzelpreis = artikel.ArtikelStatistik.DurchschnittlicherEinzelpreis,
//             DurchschnittlicherVerkaufspreis = artikel.ArtikelStatistik.DurchschnittlicherVerkaufspreis,
//             VerkaufsMenge = artikel.ArtikelStatistik.VerkaufsMenge,
//             Lagerwert = artikel.ArtikelStatistik.Lagerwert,
//             GesamtVerkaufswert = artikel.ArtikelStatistik.GesamtVerkaufswert
//         };

//         return Ok(statistik);
//     }

//     #region Helper Methods

//     private static void ApplyFilters(ref IQueryable<Domain.Entities.Artikel.Artikel> query, GetAllArtikelRequest request)
//     {
//         // Filter by name
//         if (!string.IsNullOrWhiteSpace(request.NameContains))
//         {
//             query = query.Where(a => a.Name.Contains(request.NameContains));
//         }

//         // Filter by price range
//         if (request.MinPreis.HasValue)
//         {
//             query = query.Where(a => a.Preis >= request.MinPreis.Value);
//         }

//         if (request.MaxPreis.HasValue)
//         {
//             query = query.Where(a => a.Preis <= request.MaxPreis.Value);
//         }

//         // Filter by quantity range
//         if (request.MinMenge.HasValue)
//         {
//             query = query.Where(a => a.Menge >= request.MinMenge.Value);
//         }

//         if (request.MaxMenge.HasValue)
//         {
//             query = query.Where(a => a.Menge <= request.MaxMenge.Value);
//         }

//         // Filter by status
//         if (request.StatusId.HasValue)
//         {
//             query = query.Where(a => (int)a.Status == request.StatusId.Value);
//         }

//         // Filter by stock level relative to min/max
//         if (request.UnterMindestbestand.HasValue && request.UnterMindestbestand.Value)
//         {
//             query = query.Where(a => a.Menge < a.Mindestbestand);
//         }

//         if (request.UeberMaximalbestand.HasValue && request.UeberMaximalbestand.Value)
//         {
//             query = query.Where(a => a.Menge > a.Maximalbestand);
//         }

//         // Filter by statistics
//         if (request.MinDurchschnittlicherEinzelpreis.HasValue)
//         {
//             query = query.Where(a => a.ArtikelStatistik != null &&
//                                     a.ArtikelStatistik.DurchschnittlicherEinzelpreis >= request.MinDurchschnittlicherEinzelpreis.Value);
//         }

//         if (request.MaxDurchschnittlicherEinzelpreis.HasValue)
//         {
//             query = query.Where(a => a.ArtikelStatistik != null &&
//                                     a.ArtikelStatistik.DurchschnittlicherEinzelpreis <= request.MaxDurchschnittlicherEinzelpreis.Value);
//         }

//         if (request.MinLagerwert.HasValue)
//         {
//             query = query.Where(a => a.ArtikelStatistik != null &&
//                                     a.ArtikelStatistik.Lagerwert >= request.MinLagerwert.Value);
//         }

//         if (request.MaxLagerwert.HasValue)
//         {
//             query = query.Where(a => a.ArtikelStatistik != null &&
//                                     a.ArtikelStatistik.Lagerwert <= request.MaxLagerwert.Value);
//         }
//     }

//     private static void ApplySorting(ref IQueryable<Domain.Entities.Artikel.Artikel> query, GetAllArtikelRequest request)
//     {
//         if (!string.IsNullOrWhiteSpace(request.SortBy))
//         {
//             bool isDescending = request.SortDesc.HasValue && request.SortDesc.Value;

//             query = request.SortBy.ToLower() switch
//             {
//                 "name" => isDescending ? query.OrderByDescending(a => a.Name) : query.OrderBy(a => a.Name),
//                 "preis" => isDescending ? query.OrderByDescending(a => a.Preis) : query.OrderBy(a => a.Preis),
//                 "menge" => isDescending ? query.OrderByDescending(a => a.Menge) : query.OrderBy(a => a.Menge),
//                 "status" => isDescending ? query.OrderByDescending(a => a.Status) : query.OrderBy(a => a.Status),
//                 "lagerwert" => isDescending ?
//                     query.OrderByDescending(a => a.ArtikelStatistik != null ? a.ArtikelStatistik.Lagerwert : 0) :
//                     query.OrderBy(a => a.ArtikelStatistik != null ? a.ArtikelStatistik.Lagerwert : 0),
//                 _ => isDescending ? query.OrderByDescending(a => a.Id) : query.OrderBy(a => a.Id)
//             };
//         }
//         else
//         {
//             // Default sorting by ID
//             query = query.OrderBy(a => a.Id);
//         }
//     }

//     private static void IncludeRelatedData(ref IQueryable<Domain.Entities.Artikel.Artikel> query, GetArtikelByIdRequest request)
//     {
//         // Conditionally include related data based on request
//         if (request.IncludeArtikelStatistik)
//         {
//             query = query.Include(a => a.ArtikelStatistik);
//         }

//         if (request.IncludeWareneingaenge)
//         {
//             query = query.Include(a => a.Wareneingaenge)
//                         .ThenInclude(a => a.Wareneingang);
//         }

//         if (request.IncludeWarenausgaenge)
//         {
//             query = query.Include(a => a.Warenausgaenge)
//                     .ThenInclude(w => w.Warenausgang);
//         }
//     }

//     private static GetArtikelResponse ArtikelToGetArtikelResponse(Domain.Entities.Artikel.Artikel artikel)
//     {
//         var response = new GetArtikelResponse
//         {
//             Id = artikel.Id,
//             Name = artikel.Name,
//             Preis = artikel.Preis,
//             Maximalbestand = artikel.Maximalbestand,
//             Mindestbestand = artikel.Mindestbestand,
//             Menge = artikel.Menge,
//             Status = artikel.Status,
//             BildBase64 = artikel.Bild.Length > 0 ? Convert.ToBase64String(artikel.Bild) : null
//         };

//         MapArtikelStatistik(artikel, response);
//         MapWarenausgaenge(artikel, response);
//         MapWareneingaenge(artikel, response);

//         return response;
//     }

//     private static void MapArtikelStatistik(Domain.Entities.Artikel.Artikel artikel, GetArtikelResponse response)
//     {
//         if (artikel.ArtikelStatistik != null)
//         {
//             response.Statistik = new ArtikelStatistikDto
//             {
//                 Gesamtmenge = artikel.ArtikelStatistik.Gesamtmenge,
//                 DurchschnittlicherEinzelpreis = artikel.ArtikelStatistik.DurchschnittlicherEinzelpreis,
//                 DurchschnittlicherVerkaufspreis = artikel.ArtikelStatistik.DurchschnittlicherVerkaufspreis,
//                 VerkaufsMenge = artikel.ArtikelStatistik.VerkaufsMenge,
//                 Lagerwert = artikel.ArtikelStatistik.Lagerwert,
//                 GesamtVerkaufswert = artikel.ArtikelStatistik.GesamtVerkaufswert
//             };
//         }
//     }

//     private static void MapWarenausgaenge(Domain.Entities.Artikel.Artikel artikel, GetArtikelResponse response)
//     {
//         if (artikel.Warenausgaenge != null)
//         {
//             response.WarenausgangArtikelPosition = artikel.Warenausgaenge
//                 .Select(w => new WarenausgangArtikelPositionenDto
//                 {
//                     Id = w.Id,
//                     WarenausgangId = w.WarenausgangId,
//                     ArtikelId = w.ArtikelId,
//                     ArtikelName = w.Artikel?.Name ?? "",
//                     Menge = w.Menge,
//                     Bemerkung = w.Bemerkung ?? "",
//                     Verkaufspreis = w.Verkaufspreis,
//                     Rechnungsnummer = w.Rechnungsnummer ?? "",
//                     Gesamtpreis = w.Gesamtpreis,
//                     Warenausgang = w.Warenausgang != null ? new WarenausgangDto
//                     {
//                         Id = w.Warenausgang.Id,
//                         AllgemeineBemerkungen = w.Warenausgang.AllgemeineBemerkungen ?? "",
//                         ErstelltAm = w.Warenausgang.ErstelltAm,
//                         BearbeitetAm = w.Warenausgang.BearbeitetAm,
//                         ErstelltVon = w.Warenausgang.ErstelltVon,
//                         BearbeitetVon = w.Warenausgang.BearbeitetVon,
//                         Zweck = w.Warenausgang.Zweck
//                     } : null
//                 }).ToList();
//         }
//     }

//     private static void MapWareneingaenge(Domain.Entities.Artikel.Artikel artikel, GetArtikelResponse response)
//     {
//         if (artikel.Wareneingaenge != null)
//         {
//             response.WareneingangArtikelPosition = artikel.Wareneingaenge
//                 .Select(e => new WareneingangArtikelPositionenDto
//                 {
//                     Id = e.Id,
//                     ArtikelId = e.ArtikelId,
//                     WareneingangId = e.WareneingangId,
//                     Menge = e.Menge,
//                     Einzelpreis = e.Einzelpreis,
//                     Gesamtpreis = e.Gesamtpreis,
//                     Wareneingang = e.Wareneingang != null ? new WareneingangDto
//                     {
//                         Id = e.Wareneingang.Id,
//                         AllgemeineBemerkungen = e.Wareneingang.AllgemeineBemerkungen ?? "",
//                     } : null
//                 }).ToList();
//         }
//     }

//     #endregion
// }