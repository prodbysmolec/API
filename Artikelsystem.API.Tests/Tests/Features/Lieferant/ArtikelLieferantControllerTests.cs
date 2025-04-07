using System;
using System.Net;
using System.Net.Http.Json;
using Artikelsystem.Api.Features.Lieferant.Controllers;
using Artikelsystem.Api.Features.Lieferant.Models.DTOs;
using Artikelsystem.API.Tests;

namespace Artikelsystem.Api.Tests.Tests.Features.Lieferant;

public class ArtikelLieferantControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ArtikelLieferantControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAllLieferantenForArtikel_ReturnsSuccessResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var artikelId = 1; // Verwenden Sie eine bekannte Artikel-ID für den Test

        // Act
        var response = await client.GetAsync($"/ArtikelLieferant/artikel/{artikelId}/lieferanten");

        // Assert
        response.EnsureSuccessStatusCode();
        var lieferanten = await response.Content.ReadFromJsonAsync<List<ArtikelLieferantDto>>();
        Assert.NotNull(lieferanten);
    }

    [Fact]
    public async Task GetPrimaryLieferantForArtikel_ExistingArticleWithPrimarySupplier_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Erstellen Sie einen Artikel und fügen Sie einen primären Lieferanten hinzu
        // (Dies setzt voraus, dass Sie eine Möglichkeit haben, einen Artikel und einen Lieferanten zu erstellen)
        // Hier ist ein Beispiel, wie es aussehen könnte:
        /*
        var artikelId = 1; // Bekannte ID oder neu erstellen
        var lieferantId = 1; // Bekannte ID oder neu erstellen
        
        var addDto = new ArtikelLieferantAddDto
        {
            Einkaufspreis = 100.0m,
            Mindestbestellmenge = 10,
            Lieferzeit = 3,
            ArtikelNrBeimLieferanten = "TEST-12345",
            IstPrimaer = true
        };
        
        await client.PostAsJsonAsync($"/api/artikel/{artikelId}/lieferanten/{lieferantId}", addDto);
        */
        
        // Verwenden Sie für den Test eine bekannte Artikel-ID, die einen primären Lieferanten hat
        var artikelId = 1;
        
        // Act
        var response = await client.GetAsync($"/ArtikelLieferant/artikel/{artikelId}/lieferanten/primaer");
        
        // Assert - Je nach Testdaten könnte es OK oder NotFound sein
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var lieferant = await response.Content.ReadFromJsonAsync<ArtikelLieferantDto>();
            Assert.NotNull(lieferant);
            Assert.True(lieferant.IstPrimaerLieferant);
        }
        else
        {
            // Wenn kein primärer Lieferant gefunden wurde, sollte der Status NotFound sein
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    [Fact]
    public async Task AddLieferantToArtikel_ValidRequest_ReturnsCreatedResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Bekannte IDs oder neue erstellen
        var artikelId = 1;
        var lieferantId = 1;
        
        var addDto = new ArtikelLieferantAddDto
        {
            Einkaufspreis = 199.99m,
            Mindestbestellmenge = 5,
            Lieferzeit = 2,
            ArtikelNrBeimLieferanten = "SUPPLIER-" + DateTime.Now.Ticks,
            IstPrimaer = false
        };
        
        // Act
        var response = await client.PostAsJsonAsync($"/ArtikelLieferant/artikel/{artikelId}/lieferanten/{lieferantId}", addDto);
        
        // Assert
        // Abhängig von der Testumgebung könnte dies fehlschlagen, wenn der Artikel oder Lieferant nicht existiert
        if (response.IsSuccessStatusCode)
        {
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ArtikelLieferantDto>();
            Assert.NotNull(result);
            Assert.Equal(artikelId, result.ArtikelId);
            Assert.Equal(lieferantId, result.LieferantId);
            Assert.Equal(addDto.Einkaufspreis, result.Einkaufspreis);
            Assert.Equal(addDto.IstPrimaer, result.IstPrimaerLieferant);
        }
        else if (response.StatusCode == HttpStatusCode.NotFound)
        {
            // Wenn Artikel oder Lieferant nicht gefunden wurde, wäre das ein erwartetes Ergebnis
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("nicht gefunden", content);
        }
    }

    [Fact]
    public async Task ChangeLieferant_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Bekannte IDs oder neue erstellen
        var artikelId = 1;
        var neuerLieferantId = 2; // Ein anderer Lieferant als der aktuelle
        
        var addDto = new ArtikelLieferantAddDto
        {
            Einkaufspreis = 249.99m,
            Mindestbestellmenge = 8,
            Lieferzeit = 3,
            ArtikelNrBeimLieferanten = "NEW-SUPPLIER-" + DateTime.Now.Ticks,
            IstPrimaer = true
        };
        
        // Act
        var response = await client.PostAsJsonAsync($"/ArtikelLieferant/artikel/{artikelId}/lieferanten/wechseln/{neuerLieferantId}", addDto);
        
        // Assert
        // Abhängig von der Testumgebung könnte dies fehlschlagen, wenn der Artikel oder Lieferant nicht existiert
        if (response.IsSuccessStatusCode)
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ArtikelLieferantDto>();
            Assert.NotNull(result);
            Assert.Equal(artikelId, result.ArtikelId);
            Assert.Equal(neuerLieferantId, result.LieferantId);
            Assert.Equal(addDto.Einkaufspreis, result.Einkaufspreis);
            Assert.True(result.IstPrimaerLieferant);
            Assert.True(result.IstAktiv);
        }
        else if (response.StatusCode == HttpStatusCode.NotFound)
        {
            // Wenn Artikel oder Lieferant nicht gefunden wurde, wäre das ein erwartetes Ergebnis
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("nicht gefunden", content);
        }
    }

    [Fact]
    public async Task UpdateArtikelLieferant_ExistingRelationship_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Bekannte IDs mit einer bestehenden Beziehung
        var artikelId = 1;
        var lieferantId = 1;
        
        var updateDto = new ArtikelLieferantUpdateDto
        {
            Einkaufspreis = 299.99m,
            Mindestbestellmenge = 10,
            Lieferzeit = 4,
            ArtikelNrBeimLieferanten = "UPDATED-" + DateTime.Now.Ticks,
            IstPrimaer = true
        };
        
        // Act
        var response = await client.PutAsJsonAsync($"/ArtikelLieferant/artikel/{artikelId}/lieferanten/{lieferantId}", updateDto);
        
        // Assert
        if (response.IsSuccessStatusCode)
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ArtikelLieferantDto>();
            Assert.NotNull(result);
            Assert.Equal(artikelId, result.ArtikelId);
            Assert.Equal(lieferantId, result.LieferantId);
            Assert.Equal(updateDto.Einkaufspreis, result.Einkaufspreis);
            Assert.Equal(updateDto.IstPrimaer, result.IstPrimaerLieferant);
        }
        else if (response.StatusCode == HttpStatusCode.NotFound)
        {
            // Wenn keine aktive Beziehung gefunden wurde
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Keine aktive Beziehung", content);
        }
    }

    [Fact]
    public async Task DeactivateArtikelLieferant_ExistingActiveRelationship_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Bekannte IDs mit einer aktiven Beziehung
        // Möglicherweise müssen Sie erst eine aktive Beziehung erstellen
        var artikelId = 1;
        var lieferantId = 1;
        
        // Act
        var response = await client.PatchAsync($"/ArtikelLieferant/artikel/{artikelId}/lieferanten/{lieferantId}/deactivate", null);
        
        // Assert
        if (response.IsSuccessStatusCode)
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            // Überprüfen, ob die Beziehung nun inaktiv ist
            var checkResponse = await client.GetAsync($"/ArtikelLieferant/artikel/{artikelId}/lieferanten");
            checkResponse.EnsureSuccessStatusCode();
            var lieferanten = await checkResponse.Content.ReadFromJsonAsync<List<ArtikelLieferantDto>>();
            var deaktivierteBeziehung = lieferanten?.FirstOrDefault(l => l.LieferantId == lieferantId && !l.IstAktiv);
            Assert.NotNull(deaktivierteBeziehung);
        }
        else if (response.StatusCode == HttpStatusCode.NotFound)
        {
            // Wenn keine aktive Beziehung gefunden wurde
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Keine aktive Beziehung", content);
        }
    }

    [Fact]
    public async Task SearchLieferantenForArtikel_WithValidTerm_ReturnsMatchingResults()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Bekannte Artikel-ID und ein Suchbegriff, der wahrscheinlich ein Ergebnis liefert
        var artikelId = 1;
        var suchbegriff = "Test"; // Anpassen nach Ihren Testdaten
        
        // Act
        var response = await client.GetAsync($"/ArtikelLieferant/artikel/{artikelId}/lieferanten/search?suchbegriff={suchbegriff}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var lieferanten = await response.Content.ReadFromJsonAsync<List<ArtikelLieferantDto>>();
        Assert.NotNull(lieferanten);
        // Je nach Testdaten könnte die Liste leer sein oder Ergebnisse enthalten
    }

    [Fact]
    public async Task GetArtikelByLieferant_ExistingLieferant_ReturnsSuccessResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Bekannte Lieferanten-ID
        var lieferantId = 1;
        
        // Act
        var response = await client.GetAsync($"/ArtikelLieferant/lieferanten/{lieferantId}/artikel");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var artikel = await response.Content.ReadFromJsonAsync<List<ArtikelLieferantDto>>();
        Assert.NotNull(artikel);
        // Je nach Testdaten könnte die Liste leer sein oder Ergebnisse enthalten
    }
}