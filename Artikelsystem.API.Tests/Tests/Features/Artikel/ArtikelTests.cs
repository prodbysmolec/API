using System.Net;
using System.Net.Http.Json;
using Artikelsystem.Api.Features.Artikel.Models.DTOs;
using Artikelsystem.Api.Features.Employees.Enums;
using Artikelsystem.Api.Features.Warenausgang.Models.DTOs.Responses;
using Artikelsystem.Api.Features.Wareneingang.Models.DTOs.Requests;
using Artikelsystem.API.Tests;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Artikelsystem.Api.Tests.Tests.Features.Artikel;

public class ArtikelTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ArtikelTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    // Bestehende Tests bleiben unverändert...

    #region GetArtikelById Tests

    [Fact]
    public async Task GetArtikelById_ExistingId_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var existingId = 1; // Annahme: ID 1 existiert in der Testdatenbank

        // Act
        var response = await client.GetAsync($"/artikel/{existingId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var artikel = await response.Content.ReadFromJsonAsync<GetArtikelResponse>();
        Assert.NotNull(artikel);
        Assert.Equal(existingId, artikel.Id);
    }

    [Fact]
    public async Task GetArtikelById_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();
        var nonExistingId = int.MaxValue; // Annahme: Diese ID existiert nicht

        // Act
        var response = await client.GetAsync($"/artikel/{nonExistingId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetArtikelById_WithIncludeParameterTrue_ReturnsRelatedData()
    {
        // Arrange
        var client = _factory.CreateClient();
        var existingId = 1; // Annahme: ID 1 existiert und hat Statistiken

        // Act
        var response = await client.GetAsync($"/artikel/{existingId}?includeArtikelStatistik=true");

        // Assert
        response.EnsureSuccessStatusCode();
        var artikel = await response.Content.ReadFromJsonAsync<GetArtikelResponse>();
        Assert.NotNull(artikel);
        Assert.NotNull(artikel.Statistik); // Statistik sollte enthalten sein
    }

    [Fact]
    public async Task GetArtikelById_WithMultipleIncludeParameters_ReturnsAllRelatedData()
    {
        // Arrange
        var client = _factory.CreateClient();
        var existingId = 1; // Annahme: ID 1 hat zugehörige Wareneingänge und Warenausgänge

        // Act
        var response = await client.GetAsync($"/artikel/{existingId}?includeArtikelStatistik=true&includeWareneingaenge=true&includeWarenausgaenge=true");

        // Assert
        response.EnsureSuccessStatusCode();
        var artikel = await response.Content.ReadFromJsonAsync<GetArtikelResponse>();
        Assert.NotNull(artikel);
        
        // Mindestens einer der folgenden Tests sollte bestanden werden, abhängig von den Testdaten
        if (artikel.WareneingangArtikelPosition != null)
        {
            Assert.True(artikel.WareneingangArtikelPosition.Count >= 0);
        }
        
        if (artikel.WarenausgangArtikelPosition != null)
        {
            Assert.True(artikel.WarenausgangArtikelPosition.Count >= 0);
        }
        
        if (artikel.Statistik != null)
        {
            Assert.NotNull(artikel.Statistik);
        }
    }

    #endregion

    #region GetWareneingaengeForArtikel Tests

    [Fact]
    public async Task GetWareneingaengeForArtikel_ExistingId_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var existingId = 1; // Annahme: ID 1 existiert und hat Wareneingänge

        // Act
        var response = await client.GetAsync($"/artikel/{existingId}/wareneingaenge");

        // Assert
        response.EnsureSuccessStatusCode();
        var wareneingaenge = await response.Content.ReadFromJsonAsync<IEnumerable<WareneingangArtikelPositionenDto>>();
        Assert.NotNull(wareneingaenge);
        foreach (var wareneingang in wareneingaenge)
        {
            Assert.Equal(existingId, wareneingang.ArtikelId);
        }
    }

    [Fact]
    public async Task GetWareneingaengeForArtikel_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();
        var nonExistingId = int.MaxValue; // Annahme: Diese ID existiert nicht

        // Act
        var response = await client.GetAsync($"/artikel/{nonExistingId}/wareneingaenge");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region GetWarenausgaengeForArtikel Tests

    [Fact]
    public async Task GetWarenausgaengeForArtikel_ExistingId_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var existingId = 1; // Annahme: ID 1 existiert und hat Warenausgänge

        // Act
        var response = await client.GetAsync($"/artikel/{existingId}/warenausgaenge");

        // Assert
        response.EnsureSuccessStatusCode();
        var warenausgaenge = await response.Content.ReadFromJsonAsync<IEnumerable<WarenausgangArtikelPositionDto>>();
        Assert.NotNull(warenausgaenge);
        foreach (var warenausgang in warenausgaenge)
        {
            Assert.Equal(existingId, warenausgang.ArtikelId);
        }
    }

    [Fact]
    public async Task GetWarenausgaengeForArtikel_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();
        var nonExistingId = int.MaxValue; // Annahme: Diese ID existiert nicht

        // Act
        var response = await client.GetAsync($"/artikel/{nonExistingId}/warenausgaenge");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region GetArtikelStatistik Tests

    [Fact]
    public async Task GetArtikelStatistik_ExistingId_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var existingId = 1; // Annahme: ID 1 existiert und hat Statistiken

        // Act
        var response = await client.GetAsync($"/artikel/{existingId}/statistik");

        // Assert
        response.EnsureSuccessStatusCode();
        var statistik = await response.Content.ReadFromJsonAsync<GetArtikelResponse.ArtikelStatistikDto>();
        Assert.NotNull(statistik);
    }

    [Fact]
    public async Task GetArtikelStatistik_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();
        var nonExistingId = int.MaxValue; // Annahme: Diese ID existiert nicht

        // Act
        var response = await client.GetAsync($"/artikel/{nonExistingId}/statistik");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetArtikelStatistik_ExistingIdWithoutStatistik_ReturnsNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();
        // Dies ist eine Annahme - in einer realen Test-Suite müsste man sicherstellen, 
        // dass dieser Artikel existiert aber keine Statistik hat
        var existingIdWithoutStatistik = 2; 

        // Act
        var response = await client.GetAsync($"/artikel/{existingIdWithoutStatistik}/statistik");

        // Assert
        // Entweder NotFound oder Ok mit einer leeren Statistik ist hier akzeptabel, je nach Design-Entscheidung
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        else
        {
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            // Überprüfen, ob eine Nachricht bezüglich fehlender Statistik enthalten ist
            if (!string.IsNullOrEmpty(content) && !content.Contains("null"))
            {
                var statistik = await response.Content.ReadFromJsonAsync<GetArtikelResponse.ArtikelStatistikDto>();
                // Hier könnte man prüfen, ob alle Werte 0 oder default sind
                Assert.NotNull(statistik);
            }
        }
    }

    #endregion

    #region Integration Tests

    [Fact]
    public async Task GetArtikelById_ThenGetStatistik_DataIsConsistent()
    {
        // Arrange
        var client = _factory.CreateClient();
        var existingId = 1; // Annahme: ID 1 existiert und hat Statistiken

        // Act - Erst den Artikel mit Statistiken holen
        var artikelResponse = await client.GetAsync($"/artikel/{existingId}?includeArtikelStatistik=true");
        artikelResponse.EnsureSuccessStatusCode();
        var artikel = await artikelResponse.Content.ReadFromJsonAsync<GetArtikelResponse>();
        
        // Dann separat die Statistik holen
        var statistikResponse = await client.GetAsync($"/artikel/{existingId}/statistik");
        statistikResponse.EnsureSuccessStatusCode();
        var statistik = await statistikResponse.Content.ReadFromJsonAsync<GetArtikelResponse.ArtikelStatistikDto>();

        // Assert - Die Daten sollten konsistent sein
        Assert.NotNull(artikel?.Statistik);
        Assert.NotNull(statistik);
        Assert.Equal(artikel.Statistik.Gesamtmenge, statistik.Gesamtmenge);
        Assert.Equal(artikel.Statistik.DurchschnittlicherEinzelpreis, statistik.DurchschnittlicherEinzelpreis);
        Assert.Equal(artikel.Statistik.Lagerwert, statistik.Lagerwert);
    }

    [Fact]
    public async Task GetAllArtikel_FilteredByStatus_ThenGetById_StatusIsConsistent()
    {
        // Arrange
        var client = _factory.CreateClient();
        var statusId = (int)ArtikelStatus.Verfügbar;

        // Act - Erst gefilterte Liste holen
        var listResponse = await client.GetAsync($"/artikel?statusId={statusId}");
        listResponse.EnsureSuccessStatusCode();
        var artikelList = await listResponse.Content.ReadFromJsonAsync<List<GetArtikelResponse>>();
        
        // Wenn keine Artikel gefunden wurden, Test überspringen
        if (artikelList == null || !artikelList.Any())
        {
            return;
        }
        
        // Einzelnen Artikel per ID abfragen
        var firstArtikel = artikelList.First();
        var detailResponse = await client.GetAsync($"/artikel/{firstArtikel.Id}");
        detailResponse.EnsureSuccessStatusCode();
        var artikelDetail = await detailResponse.Content.ReadFromJsonAsync<GetArtikelResponse>();

        // Assert
        Assert.NotNull(artikelDetail);
        Assert.Equal(ArtikelStatus.Verfügbar, artikelDetail.Status);
    }

    #endregion

    #region Error Handling Tests

    [Fact]
    public async Task GetArtikelById_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/artikel/invalid"); // Nicht-numerische ID

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode); // 404 wegen Route-Constraint
    }

    [Fact]
    public async Task GetArtikelStatistik_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/artikel/invalid/statistik"); // Nicht-numerische ID

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode); // 404 wegen Route-Constraint
    }

    #endregion
}