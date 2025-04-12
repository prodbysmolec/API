// using System;
// using Artikelsystem.Shared.DTOs.Lieferant;
// using Application.Interfaces;
// using API.Common.Controllers;
// using FluentValidation;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Http.HttpResults;

// namespace API.Controller;


// public class ArtikelLieferantController : BaseController
// {
//     private readonly IArtikelLieferantService _service;
//     private readonly ILogger<ArtikelLieferantController> _logger;

//     public ArtikelLieferantController(
//         IArtikelLieferantService service,
//         ILogger<ArtikelLieferantController> logger)
//     {
//         _logger = logger;
//         _service = service;
//     }

//     /// <summary>
//     /// Holt alle Lieferanten für einen Artikel
//     /// </summary>
//     /// <param name="artikelId">Die ID des Artikels</param>
//     /// <param name="nurAktive">Optional: Nur aktive Lieferanten zurückgeben</param>
//     [HttpGet("artikel/{artikelId:int}/lieferanten")]
//     [ProducesResponseType(typeof(List<ArtikelLieferantDto>), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> GetAllLieferantenForArtikel(int artikelId, [FromQuery] bool nurAktive = false)
//     {
//         try
//         {
//             var lieferanten = await _service.GetAllLieferantenForArtikelAsync(artikelId, nurAktive);
//             return Ok(lieferanten);
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "Fehler beim Abrufen der Lieferanten für Artikel {ArtikelId}", artikelId);
//             return StatusCode(500, "Ein Fehler ist aufgetreten");
//         }
//     }

//     /// <summary>
//     /// Holt den primären Lieferanten für einen Artikel
//     /// </summary>
//     /// <param name="artikelId">Die ID des Artikels</param>
//     [HttpGet("artikel/{artikelId:int}/lieferanten/primaer")]
//     [ProducesResponseType(typeof(ArtikelLieferantDto), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> GetPrimaryLieferantForArtikel(int artikelId)
//     {
//         try
//         {
//             var lieferant = await _service.GetPrimaryLieferantForArtikelAsync(artikelId);
            
//             if (lieferant == null)
//             {
//                 return NotFound($"Kein primärer Lieferant für Artikel {artikelId} gefunden");
//             }
            
//             return Ok(lieferant);
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "Fehler beim Abrufen des primären Lieferanten für Artikel {ArtikelId}", artikelId);
//             return StatusCode(500, "Ein Fehler ist aufgetreten");
//         }
//     }

//     /// <summary>
//     /// Fügt einen neuen Lieferanten für einen Artikel hinzu
//     /// </summary>
//     /// <param name="artikelId">Die ID des Artikels</param>
//     /// <param name="lieferantId">Die ID des Lieferanten</param>
//     /// <param name="dto">Die Daten der Lieferanten-Artikel-Beziehung</param>
//     [HttpPost("artikel/{artikelId:int}/lieferanten/{lieferantId:int}")]
//     [ProducesResponseType(typeof(ArtikelLieferantDto), StatusCodes.Status201Created)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> AddLieferantToArtikel(int artikelId, int lieferantId, ArtikelLieferantAddDto dto)
//     {
//         try
//         {
//             var artikelLieferant = await _service.AddLieferantToArtikelAsync(artikelId, lieferantId, dto);
//             return CreatedAtAction(
//                 nameof(GetAllLieferantenForArtikel),
//                 new { artikelId },
//                 artikelLieferant
//             );
//         }
//         catch (KeyNotFoundException ex)
//         {
//             return NotFound(ex.Message);
//         }
//         catch (ValidationException ex)
//         {
//             return BadRequest(ex.Errors);
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "Fehler beim Hinzufügen eines Lieferanten {LieferantId} zum Artikel {ArtikelId}", lieferantId, artikelId);
//             return StatusCode(500, "Ein Fehler ist aufgetreten");
//         }
//     }

//     /// <summary>
//     /// Wechselt den Lieferanten für einen Artikel
//     /// </summary>
//     /// <param name="artikelId">Die ID des Artikels</param>
//     /// <param name="neuerLieferantId">Die ID des neuen Lieferanten</param>
//     /// <param name="dto">Die Daten der Lieferanten-Artikel-Beziehung</param>
//     [HttpPost("artikel/{artikelId:int}/lieferanten/wechseln/{neuerLieferantId:int}")]
//     [ProducesResponseType(typeof(ArtikelLieferantDto), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//     public async Task<IActionResult> ChangeLieferant(int artikelId, int neuerLieferantId, ArtikelLieferantAddDto dto)
//     {
//         try
//         {
//             var artikelLieferant = await _service.ChangeLieferantAsync(artikelId, neuerLieferantId, dto);
//             return Ok(artikelLieferant);
//         }
//         catch (KeyNotFoundException ex)
//         {
//             return NotFound(ex.Message);
//         }
//         catch (ValidationException ex)
//         {
//             return BadRequest(ex.Errors);
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "Fehler beim Wechseln des Lieferanten für Artikel {ArtikelId} zu {LieferantId}", artikelId, neuerLieferantId);
//             return StatusCode(500, "Ein Fehler ist aufgetreten");
//         }
//     }


//         /// <summary>
//         /// Aktualisiert die Lieferantenbeziehung zu einem Artikel
//         /// </summary>
//         /// <param name="artikelId">Die ID des Artikels</param>
//         /// <param name="lieferantId">Die ID des Lieferanten</param>
//         /// <param name="dto">Die aktualisierten Daten der Lieferanten-Artikel-Beziehung</param>
//         [HttpPut("artikel/{artikelId:int}/lieferanten/{lieferantId:int}")]
//         [ProducesResponseType(typeof(ArtikelLieferantDto), StatusCodes.Status200OK)]
//         [ProducesResponseType(StatusCodes.Status400BadRequest)]
//         [ProducesResponseType(StatusCodes.Status404NotFound)]
//         [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//         public async Task<IActionResult> UpdateArtikelLieferant(int artikelId, int lieferantId, ArtikelLieferantUpdateDto dto)
//         {
//             try
//             {
//                 var artikelLieferant = await _service.UpdateArtikelLieferantAsync(artikelId, lieferantId, dto);
                
//                 if (artikelLieferant == null)
//                 {
//                     return NotFound($"Keine aktive Beziehung zwischen Artikel {artikelId} und Lieferant {lieferantId} gefunden");
//                 }
                
//                 return Ok(artikelLieferant);
//             }
//             catch (ValidationException ex)
//             {
//                 return BadRequest(ex.Errors);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Fehler beim Aktualisieren der Beziehung zwischen Artikel {ArtikelId} und Lieferant {LieferantId}", artikelId, lieferantId);
//                 return StatusCode(500, "Ein Fehler ist aufgetreten");
//             }
//         }



//         /// <summary>
//         /// Deaktiviert eine Lieferantenbeziehung zu einem Artikel
//         /// </summary>
//         /// <param name="artikelId">Die ID des Artikels</param>
//         /// <param name="lieferantId">Die ID des Lieferanten</param>
//         [HttpPatch("artikel/{artikelId:int}/lieferanten/{lieferantId:int}/deactivate")]
//         [ProducesResponseType(StatusCodes.Status200OK)]
//         [ProducesResponseType(StatusCodes.Status404NotFound)]
//         [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//         public async Task<IActionResult> DeactivateArtikelLieferant(int artikelId, int lieferantId)
//         {
//             try
//             {
//                 var result = await _service.DeactivateArtikelLieferantAsync(artikelId, lieferantId);
                
//                 if (!result)
//                 {
//                     return NotFound($"Keine aktive Beziehung zwischen Artikel {artikelId} und Lieferant {lieferantId} gefunden");
//                 }
                
//                 return Ok($"Beziehung zwischen Artikel {artikelId} und Lieferant {lieferantId} wurde deaktiviert");
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Fehler beim Deaktivieren der Beziehung zwischen Artikel {ArtikelId} und Lieferant {LieferantId}", artikelId, lieferantId);
//                 return StatusCode(500, "Ein Fehler ist aufgetreten");
//             }
//         }


//         /// <summary>
//         /// Löscht eine Lieferantenbeziehung zu einem Artikel
//         /// </summary>
//         /// <param name="artikelId">Die ID des Artikels</param>
//         /// <param name="lieferantId">Die ID des Lieferanten</param>
//         [HttpDelete("artikel/{artikelId:int}/lieferanten/{lieferantId:int}")]
//         [ProducesResponseType(StatusCodes.Status200OK)]
//         [ProducesResponseType(StatusCodes.Status404NotFound)]
//         [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//         public async Task<IActionResult> DeleteArtikelLieferant(int artikelId, int lieferantId)
//         {
//             try
//             {
//                 var result = await _service.DeleteArtikelLieferantAsync(artikelId, lieferantId);
                
//                 if (!result)
//                 {
//                     return NotFound($"Keine Beziehung zwischen Artikel {artikelId} und Lieferant {lieferantId} gefunden");
//                 }
                
//                 return Ok($"Beziehung zwischen Artikel {artikelId} und Lieferant {lieferantId} wurde gelöscht");
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Fehler beim Löschen der Beziehung zwischen Artikel {ArtikelId} und Lieferant {LieferantId}", artikelId, lieferantId);
//                 return StatusCode(500, "Ein Fehler ist aufgetreten");
//             }
//         }


//         /// <summary>
//         /// Sucht nach Lieferanten für einen bestimmten Artikel anhand von Suchkriterien
//         /// </summary>
//         /// <param name="artikelId">Die ID des Artikels</param>
//         /// <param name="suchbegriff">Der Suchbegriff</param>
//         [HttpGet("artikel/{artikelId:int}/lieferanten/search")]
//         [ProducesResponseType(typeof(List<ArtikelLieferantDto>), StatusCodes.Status200OK)]
//         [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//         public async Task<IActionResult> SearchLieferantenForArtikel(int artikelId, [FromQuery] string suchbegriff)
//         {
//             try
//             {
//                 var lieferanten = await _service.SearchLieferantenForArtikelAsync(artikelId, suchbegriff);
//                 return Ok(lieferanten);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Fehler bei der Suche nach Lieferanten für Artikel {ArtikelId} mit Suchbegriff {Suchbegriff}", artikelId, suchbegriff);
//                 return StatusCode(500, "Ein Fehler ist aufgetreten");
//             }
//         }

//         /// <summary>
//         /// Holt alle Artikelbeziehungen für einen bestimmten Lieferanten
//         /// </summary>
//         /// <param name="lieferantId">Die ID des Lieferanten</param>
//         /// <param name="nurAktive">Optional: Nur aktive Beziehungen zurückgeben</param>
//         [HttpGet("lieferanten/{lieferantId:int}/artikel")]
//         [ProducesResponseType(typeof(List<ArtikelLieferantDto>), StatusCodes.Status200OK)]
//         [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//         public async Task<IActionResult> GetArtikelByLieferant(int lieferantId, [FromQuery] bool nurAktive = true)
//         {
//             try
//             {
//                 var artikel = await _service.GetArtikelByLieferantAsync(lieferantId, nurAktive);
//                 return Ok(artikel);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Fehler beim Abrufen der Artikel für Lieferant {LieferantId}", lieferantId);
//                 return StatusCode(500, "Ein Fehler ist aufgetreten");
//             }
//         }
// }

// public class ArtikelLieferantAddDto
// {
//     public decimal Einkaufspreis { get; set; }
//     public bool IstPrimaer { get; set; }
//     public int? Mindestbestellmenge { get; set; }
//     public int? Lieferzeit { get; set; }
//     public string? ArtikelNrBeimLieferanten { get; set; }
// }
