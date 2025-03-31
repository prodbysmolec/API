using Artikelsystem.Api.Features.Artikel.Models.DTOs;
using Artikelsystem.Api.Features.Employees.Enums;
using Artikelsystem.Api.Features.Employees.Models.DTOs;
using Artikelsystem.Api.Features.Employees.Models.Entitys;
using Artikelsystem.Api.Infrastructure.Persistence.Context;
using Artikelsystem.API.Shared.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;

namespace Artikelsystem.Api.Features.Artikel.Controllers;


public class ArtikelController : BaseController
{
    // private readonly IArtikelService _artikelService;
    private readonly ILogger<ArtikelController> _logger;
    private readonly AppDbContext _dbContext;

    public ArtikelController(
        ILogger<ArtikelController> logger,
        AppDbContext dbContext
    )
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    /// <summary>
    /// Gets all articles in the system with filtering options.
    /// </summary>
    /// <param name="request">Filter and pagination parameters</param>
    /// <returns>Returns the filtered articles in a JSON array.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetArtikelResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<GetArtikelResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllArtikel([FromQuery] GetAllArtikelRequest? request)
    {
        int page = request?.Page ?? 1;
        int recordsPerPage = request?.RecordsPerPage ?? 100;

        // Start with base query including related data
        IQueryable<Models.Entitys.Artikel> query = _dbContext.Artikel
            .Include(a => a.ArtikelStatistik);

        // Apply filters if request is not null
        if (request != null)
        {
            // Filter by name
            if (!string.IsNullOrWhiteSpace(request.NameContains))
            {
                query = query.Where(a => a.Name.Contains(request.NameContains));
            }

            // Filter by price range
            if (request.MinPreis.HasValue)
            {
                query = query.Where(a => a.Preis >= request.MinPreis.Value);
            }
            
            if (request.MaxPreis.HasValue)
            {
                query = query.Where(a => a.Preis <= request.MaxPreis.Value);
            }

            // Filter by quantity range
            if (request.MinMenge.HasValue)
            {
                query = query.Where(a => a.Menge >= request.MinMenge.Value);
            }
            
            if (request.MaxMenge.HasValue)
            {
                query = query.Where(a => a.Menge <= request.MaxMenge.Value);
            }

            // Filter by status
            if (request.StatusId.HasValue)
            {
                query = query.Where(a => (int)a.Status == request.StatusId.Value);
            }

            // Filter by stock level relative to min/max
            if (request.UnterMindestbestand.HasValue && request.UnterMindestbestand.Value)
            {
                query = query.Where(a => a.Menge < a.Mindestbestand);
            }
            
            if (request.UeberMaximalbestand.HasValue && request.UeberMaximalbestand.Value)
            {
                query = query.Where(a => a.Menge > a.Maximalbestand);
            }
            
            // Filter by statistics
            if (request.MinDurchschnittlicherEinzelpreis.HasValue)
            {
                query = query.Where(a => a.ArtikelStatistik != null && 
                                        a.ArtikelStatistik.DurchschnittlicherEinzelpreis >= request.MinDurchschnittlicherEinzelpreis.Value);
            }
            
            if (request.MaxDurchschnittlicherEinzelpreis.HasValue)
            {
                query = query.Where(a => a.ArtikelStatistik != null && 
                                        a.ArtikelStatistik.DurchschnittlicherEinzelpreis <= request.MaxDurchschnittlicherEinzelpreis.Value);
            }
            
            if (request.MinLagerwert.HasValue)
            {
                query = query.Where(a => a.ArtikelStatistik != null && 
                                        a.ArtikelStatistik.Lagerwert >= request.MinLagerwert.Value);
            }
            
            if (request.MaxLagerwert.HasValue)
            {
                query = query.Where(a => a.ArtikelStatistik != null && 
                                        a.ArtikelStatistik.Lagerwert <= request.MaxLagerwert.Value);
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                bool isDescending = request.SortDesc.HasValue && request.SortDesc.Value;
                
                query = request.SortBy.ToLower() switch
                {
                    "name" => isDescending ? query.OrderByDescending(a => a.Name) : query.OrderBy(a => a.Name),
                    "preis" => isDescending ? query.OrderByDescending(a => a.Preis) : query.OrderBy(a => a.Preis),
                    "menge" => isDescending ? query.OrderByDescending(a => a.Menge) : query.OrderBy(a => a.Menge),
                    "status" => isDescending ? query.OrderByDescending(a => a.Status) : query.OrderBy(a => a.Status),
                    "lagerwert" => isDescending ? 
                        query.OrderByDescending(a => a.ArtikelStatistik != null ? a.ArtikelStatistik.Lagerwert : 0) : 
                        query.OrderBy(a => a.ArtikelStatistik != null ? a.ArtikelStatistik.Lagerwert : 0),
                    _ => isDescending ? query.OrderByDescending(a => a.Id) : query.OrderBy(a => a.Id)
                };
            }
            else
            {
                // Default sorting by ID
                query = query.OrderBy(a => a.Id);
            }
        }

        // Apply pagination after all filters
        query = query.Skip((page - 1) * recordsPerPage).Take(recordsPerPage);

        var artikel = await query.ToArrayAsync();

        return Ok(artikel.Select(ArtikelToGetArtikelResponse));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetArtikelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetArtikelById(int id, [FromQuery] GetArtikelByIdRequest? request = null)
    {
        request ??= new GetArtikelByIdRequest();        

        // Start with base query
        IQueryable<Models.Entitys.Artikel> query = _dbContext.Artikel;
        
        // Conditionally include related data based on request
        if (request.IncludeArtikelStatistik)
        {
            query = query.Include(a => a.ArtikelStatistik);
        }
        
        // if (request.IncludeLieferanten)
        // {
        //     query = query.Include(a => a.Wareneingaenge);
        // }
        
        if (request.IncludeWareneingänge)
        {
            query = query.Include(a => a.Wareneingaenge)
                        .ThenInclude(a => a.Wareneingang);
        }
        // Filter by ID
        var artikel = await query.SingleOrDefaultAsync(a => a.Id == id);
        
        if(artikel == null) return NotFound();

        // Use your existing mapper method
        var artikelResponse = ArtikelToGetArtikelResponse(artikel);

        return Ok(artikelResponse);  // Return artikelResponse instead of artikel


        // var employee = await _dbContext.Artikel.SingleOrDefaultAsync(e => e.Id == id);
        // if (employee == null)
        // {
        //     return NotFound();
        // }

        // var employeeResponse = ArtikelToGetArtikelResponse(employee);
    }


    // /// <summary>
    // /// Holt einen Artikel anhand der ID mit Optionen zum Filtern der zurückgegebenen Daten.
    // /// </summary>
    // /// <param name="id">Die ID des Artikels.</param>
    // /// <returns>Der Artikel Record mit den angeforderten Daten.</returns>
    // [HttpGet("{id:int}")]
    // [ProducesResponseType(typeof(GetArtikelResponse), StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // public async Task<IActionResult> GetArtikelById(int id)
    // {
    //     _logger.LogInformation("Hole Artikel mit ID: {ArtikelId}", id);
    //     var employee = await _dbContext.Artikel.SingleOrDefaultAsync(e => e.Id == id);
    //     if (employee == null)
    //     {
    //         return NotFound();
    //     }

    //     var employeeResponse = ArtikelToGetArtikelResponse(employee);

    //     return Ok(employeeResponse);
    //     //     request ??= new GetArtikelByIdRequest();

    //     //     // Starte die Query
    //     //     IQueryable<Models.Entitys.Artikel> query = _dbContext.Artikel;

    //     //     // Bedingtes einbinden der Anfrage basierend auf request
    //     //     if(request.IncludeArtikelStatistik)
    //     //     {
    //     //         query = query.Include(a => a.ArtikelStatistik);
    //     //     }

    //     //     // if(request.IncludeLieferanten)
    //     //     // {
    //     //     //     query = query.Include(a => a.Lieferant);
    //     //     // }

    //     //     // kommt später
    //     //     // if(request.IncludeArtikelGruppen)
    //     //     // {
    //     //     //     query = query.Include(a => a.ArtikelGruppen);
    //     //     // }

    //     //     if(request.IncludeWareneingänge)
    //     //     {
    //     //         query = query.Include(a => a.Wareneingaenge);
    //     //     }

    //     //     // if(request.IncludeArtikelZusatzwerte)
    //     //     // {
    //     //     //     query = query
    //     //     //         .Include(a => a.ArtikelZusatzWerte)
    //     //     //         .ThenInclude(az => az.Zusatzwert)
    //     //     //         .ThenInclude(z => z.Zusatzwert);
    //     //     // }

    //     //     // Filter basierend auf ID 
    //     //     var artikel = await query.SingleOrDefaultAsync(a => a.Id == id);

    //     //     if(artikel == null) return NotFound(); 

    //     //     var artikelResponse = ArtikelToGetArtikelResponse(artikel);

    //     //     return Ok(artikelResponse);
    //     // }
    // }

    private static GetArtikelResponse ArtikelToGetArtikelResponse(Models.Entitys.Artikel artikel)
    {
        var response = new GetArtikelResponse
        {
            Id = artikel.Id,
            Name = artikel.Name,
            Preis = artikel.Preis,
            Maximalbestand = artikel.Maximalbestand,
            Mindestbestand = artikel.Mindestbestand,
            Menge = artikel.Menge,
            Status = artikel.Status,
            BildBase64 = artikel.Bild.Length > 0 ? Convert.ToBase64String(artikel.Bild) : null
        };

        if (artikel.ArtikelStatistik != null)
        {
            response.Statistik = new GetArtikelResponse.ArtikelStatistikDto
            {
                Gesamtmenge = artikel.ArtikelStatistik.Gesamtmenge,
                DurchschnittlicherEinzelpreis = artikel.ArtikelStatistik.DurchschnittlicherEinzelpreis,
                DurchschnittlicherVerkaufspreis = artikel.ArtikelStatistik.DurchschnittlicherVerkaufspreis,
                VerkaufsMenge = artikel.ArtikelStatistik.VerkaufsMenge,
                Lagerwert = artikel.ArtikelStatistik.Lagerwert,
                GesamtVerkaufswert = artikel.ArtikelStatistik.GesamtVerkaufswert
            };
        }

        return response;
    }

}