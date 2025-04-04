using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Artikelsystem.Api.Features.Inventur.Models.Dtos;
using Artikelsystem.Api.Features.Inventur.Models.Enums;
using Artikelsystem.API.Tests;
using Xunit;

namespace Artikelsystem.Api.Tests.Features.Inventur;

public class InventurControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public InventurControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    #region Inventur Grundoperationen

    [Fact]
    public async Task GetAlleInventuren_ReturnsSuccessResult()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/inventur");

        // Assert
        response.EnsureSuccessStatusCode();
        var inventuren = await response.Content.ReadFromJsonAsync<List<InventurDto>>();
        Assert.NotNull(inventuren);
    }

    [Fact]
    public async Task GetInventurById_ExistingId_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Erst eine neue Inventur erstellen, um eine bekannte ID zu haben
        var createRequest = new CreateInventurRequest
        {
            Bezeichnung = "Test Inventur für GetById",
            Bemerkung = "Test Bemerkung",
            ErstelltVon = "IntegrationTest"
        };
        
        var createResponse = await client.PostAsJsonAsync("/inventur", createRequest);
        createResponse.EnsureSuccessStatusCode();
        var createdInventur = await createResponse.Content.ReadFromJsonAsync<InventurDto>();
        Assert.NotNull(createdInventur);

        // Act
        var response = await client.GetAsync($"/inventur/{createdInventur.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var inventur = await response.Content.ReadFromJsonAsync<InventurDto>();
        Assert.NotNull(inventur);
        Assert.Equal(createdInventur.Id, inventur.Id);
        Assert.Equal(createRequest.Bezeichnung, inventur.Bezeichnung);
    }

    [Fact]
    public async Task GetInventurById_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();
        var nonExistingId = int.MaxValue; // Diese ID existiert mit hoher Wahrscheinlichkeit nicht

        // Act
        var response = await client.GetAsync($"/inventur/{nonExistingId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ErstelleInventur_ValidRequest_ReturnsCreatedResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new CreateInventurRequest
        {
            Bezeichnung = "Neue Test-Inventur " + DateTime.Now.Ticks,
            Bemerkung = "Erstellt im Integrationstest",
            ErstelltVon = "prodbysmolec"
        };

        // Act
        var response = await client.PostAsJsonAsync("/inventur", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var inventur = await response.Content.ReadFromJsonAsync<InventurDto>();
        Assert.NotNull(inventur);
        Assert.Equal(request.Bezeichnung, inventur.Bezeichnung);
        Assert.Equal(request.Bemerkung, inventur.Bemerkung);
        Assert.Equal(InventurStatus.Erstellt, inventur.Status);
    }

    [Fact]
    public async Task StarteInventur_ExistingIdWithStatusErstellt_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        await LoescheAlleInventuren(client);
        // Erst eine neue Inventur erstellen
        var createRequest = new CreateInventurRequest
        {
            Bezeichnung = "Zu startende Inventur " + DateTime.Now.Ticks,
            Bemerkung = "Wird im Test gestartet",
            ErstelltVon = "prodbysmolec"
        };
        
        var createResponse = await client.PostAsJsonAsync("/inventur", createRequest);
        createResponse.EnsureSuccessStatusCode();
        var createdInventur = await createResponse.Content.ReadFromJsonAsync<InventurDto>();
        Assert.NotNull(createdInventur);

        // Act
        var response = await client.PostAsync($"/inventur/{createdInventur.Id}/starten", null);

        // Assert
        response.EnsureSuccessStatusCode();
        var inventur = await response.Content.ReadFromJsonAsync<InventurDto>();
        var inventurResponse = await client.GetAsync($"/inventur/{createdInventur.Id}");
        var inventur2 = await inventurResponse.Content.ReadFromJsonAsync<InventurDto>();

        Assert.NotNull(inventur);
        Assert.Equal(InventurStatus.InBearbeitung, inventur.Status);
        //Assert.NotEmpty(inventur.Positionen);
    }

    [Fact]
    public async Task AktualisieereInventurPosition_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        await LoescheAlleInventuren(client);
        // Erst eine Inventur erstellen und starten
        var createRequest = new CreateInventurRequest
        {
            Bezeichnung = "Inventur für Positionsupdate " + DateTime.Now.Ticks,
            Bemerkung = "Positionen werden aktualisiert",
            ErstelltVon = "prodbysmolec"
        };
        
        var createResponse = await client.PostAsJsonAsync("/inventur", createRequest);
        createResponse.EnsureSuccessStatusCode();
        var createdInventur = await createResponse.Content.ReadFromJsonAsync<InventurDto>();
        
        var startResponse = await client.PostAsync($"/inventur/{createdInventur?.Id}/starten", null);
        startResponse.EnsureSuccessStatusCode();
        var startedInventur = await startResponse.Content.ReadFromJsonAsync<InventurDto>();
        
        Assert.NotNull(startedInventur);
        //Assert.NotEmpty(startedInventur.Positionen);
        
        var position = startedInventur.Positionen[0];
        var updateRequest = new UpdateInventurPositionRequest
        {
            PositionId = position.Id,
            ArtikelId = position.ArtikelId,
            InventurID = startedInventur.Id,
            GezaehlteMenge = position.SystemMenge + 3, // 3 Stück mehr als im System
            Bemerkung = "Im Test gezählt und aktualisiert",
            BearbeitetVon = "prodbysmolec",
            IstGeprueft = true
        };

        // Act
        var response = await client.PutAsJsonAsync("/inventur/positionen", updateRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        var updatedPosition = await response.Content.ReadFromJsonAsync<InventurPositionDto>();
        Assert.NotNull(updatedPosition);
        Assert.Equal(position.Id, updatedPosition.Id);
        Assert.Equal(updateRequest.GezaehlteMenge, updatedPosition.GezaehlteMenge);
        Assert.Equal(updateRequest.Bemerkung, updatedPosition.Bemerkung);
        Assert.True(updatedPosition.IstGeprueft);
        Assert.Equal(3, updatedPosition.Differenz); // Differenz sollte 3 sein
    }

    #endregion

    #region Inventur Workflow und Abschluss

    [Fact]
    public async Task CompleteInventurProcess_CreationToClosingWithBericht_Success()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        await LoescheAlleInventuren(client);

        // 1. Inventur erstellen
        var createRequest = new CreateInventurRequest
        {
            Bezeichnung = "Vollständiger Prozess-Test " + DateTime.Now.Ticks,
            Bemerkung = "Test des kompletten Inventur-Lebenszyklus",
            ErstelltVon = "prodbysmolec"
        };
        
        var createResponse = await client.PostAsJsonAsync("/inventur", createRequest);
        createResponse.EnsureSuccessStatusCode();
        var createdInventur = await createResponse.Content.ReadFromJsonAsync<InventurDto>();
        Assert.NotNull(createdInventur);
        
        // 2. Inventur starten
        var startResponse = await client.PostAsync($"/inventur/{createdInventur.Id}/starten", null);
        startResponse.EnsureSuccessStatusCode();
        var startedInventur = await startResponse.Content.ReadFromJsonAsync<InventurDto>();
        Assert.NotNull(startedInventur);
        //Assert.NotEmpty(startedInventur.Positionen);
        
        // 3. Alle Positionen aktualisieren
        foreach (var position in startedInventur.Positionen)
        {
            var updateRequest = new UpdateInventurPositionRequest
            {
                PositionId = position.Id,
                ArtikelId = position.ArtikelId,
                InventurID = startedInventur.Id,
                GezaehlteMenge = position.SystemMenge + (position.Id % 2 == 0 ? 2 : -2), // Abwechselnd +2 oder -2
                Bemerkung = "Erfasst im vollständigen Workflow-Test",
                BearbeitetVon = "prodbysmolec",
                IstGeprueft = true
            };
            
            var updateResponse = await client.PutAsJsonAsync("/inventur/positionen", updateRequest);
            updateResponse.EnsureSuccessStatusCode();
        }
        
        // 4. Inventur abschließen
        var closeResponse = await client.PostAsync($"/inventur/{createdInventur.Id}/abschliessen", null);
        closeResponse.EnsureSuccessStatusCode();
        var closedInventur = await closeResponse.Content.ReadFromJsonAsync<InventurDto>();
        Assert.NotNull(closedInventur);
        Assert.Equal(InventurStatus.Abgeschlossen, closedInventur.Status);
        Assert.NotNull(closedInventur.AbschlussDatum);
        
        // 5. Inventurbericht abrufen
        var berichtResponse = await client.GetAsync($"/inventur/{createdInventur.Id}/bericht");
        berichtResponse.EnsureSuccessStatusCode();
        var bericht = await berichtResponse.Content.ReadFromJsonAsync<InventurBerichtDto>();
        
        // Assert Bericht
        Assert.NotNull(bericht);
        Assert.Equal(createdInventur.Id, bericht.InventurId);
        Assert.NotEmpty(bericht.Inhalt);
        Assert.True(bericht.GesamtDifferenzWert != 0, "Der Bericht sollte einen Differenzwert ungleich Null haben");
        Assert.True(bericht.AnzahlPositionenMitDifferenz > 0, "Es sollten Positionen mit Differenzen im Bericht sein");
    }

    [Fact]
    public async Task SchliesseInventurAb_NotAllPositionsChecked_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // 1. Inventur erstellen
        var createRequest = new CreateInventurRequest
        {
            Bezeichnung = "Unvollständige Inventur " + DateTime.Now.Ticks,
            Bemerkung = "Einige Positionen bleiben ungeprüft",
            ErstelltVon = "prodbysmolec"
        };
        
        var createResponse = await client.PostAsJsonAsync("/inventur", createRequest);
        createResponse.EnsureSuccessStatusCode();
        var createdInventur = await createResponse.Content.ReadFromJsonAsync<InventurDto>();
        
        // 2. Inventur starten
        var startResponse = await client.PostAsync($"/inventur/{createdInventur!.Id}/starten", null);
        startResponse.EnsureSuccessStatusCode();
        var startedInventur = await startResponse.Content.ReadFromJsonAsync<InventurDto>();
        
        // 3. Nur einen Teil der Positionen aktualisieren (erste Position)
        if (startedInventur?.Positionen.Count > 0)
        {
            var position = startedInventur.Positionen[0];
            var updateRequest = new UpdateInventurPositionRequest
            {
                PositionId = position.Id,
                ArtikelId = position.ArtikelId,
                InventurID = startedInventur.Id,
                GezaehlteMenge = position.SystemMenge,
                Bemerkung = "Nur diese Position aktualisiert",
                BearbeitetVon = "prodbysmolec",
                IstGeprueft = true
            };
            
            var updateResponse = await client.PutAsJsonAsync("/inventur/positionen", updateRequest);
            updateResponse.EnsureSuccessStatusCode();
        }
        
        // 4. Versuch, die Inventur abzuschließen
        var closeResponse = await client.PostAsync($"/inventur/{createdInventur.Id}/abschliessen", null);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, closeResponse.StatusCode);
    }

    #endregion

    #region Inventurbericht Tests

    [Fact]
    public async Task GetInventurBerichte_ReturnsAllBerichte()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/inventur/berichte");

        // Assert
        response.EnsureSuccessStatusCode();
        var berichte = await response.Content.ReadFromJsonAsync<List<InventurBerichtDto>>();
        Assert.NotNull(berichte);
    }

    [Fact]
    public async Task GetInventurBericht_ForExistingInventur_ReturnsBericht()
    {
        // Arrange
        var client = _factory.CreateClient();
        await LoescheAlleInventuren(client);
        // Erst vollständigen Inventurprozess durchführen, um einen Bericht zu haben
        // 1. Inventur erstellen
        var createRequest = new CreateInventurRequest
        {
            Bezeichnung = "Inventur für Berichtstest " + DateTime.Now.Ticks,
            Bemerkung = "Bericht wird getestet",
            ErstelltVon = "prodbysmolec"
        };
        
        var createResponse = await client.PostAsJsonAsync("/inventur", createRequest);
        createResponse.EnsureSuccessStatusCode();
        var createdInventur = await createResponse.Content.ReadFromJsonAsync<InventurDto>();
        
        // 2. Inventur starten
        var startResponse = await client.PostAsync($"/inventur/{createdInventur?.Id}/starten", null);
        startResponse.EnsureSuccessStatusCode();
        var startedInventur = await startResponse.Content.ReadFromJsonAsync<InventurDto>();
        
        // 3. Alle Positionen aktualisieren
        foreach (var position in startedInventur!.Positionen)
        {
            var updateRequest = new UpdateInventurPositionRequest
            {
                PositionId = position.Id,
                ArtikelId = position.ArtikelId,
                InventurID = startedInventur.Id,
                GezaehlteMenge = position.SystemMenge + 1, // Jede Position +1
                Bemerkung = "Position für Berichtstest",
                BearbeitetVon = "prodbysmolec",
                IstGeprueft = true
            };
            
            await client.PutAsJsonAsync("/inventur/positionen", updateRequest);
        }
        
        // 4. Inventur abschließen
        var closeResponse = await client.PostAsync($"/inventur/{createdInventur!.Id}/abschliessen", null);
        closeResponse.EnsureSuccessStatusCode();
        
        // Act - Bericht abrufen
        var berichtResponse = await client.GetAsync($"/inventur/{createdInventur.Id}/bericht");
        
        // Assert
        berichtResponse.EnsureSuccessStatusCode();
        var bericht = await berichtResponse.Content.ReadFromJsonAsync<InventurBerichtDto>();
        Assert.NotNull(bericht);
        Assert.Equal(createdInventur.Id, bericht.InventurId);
        Assert.NotEmpty(bericht.Inhalt);
        Assert.True(bericht.GesamtDifferenzWert > 0, "Der Bericht sollte positive Differenzwerte haben");
    }

    [Fact]
    public async Task GetBerichtById_ValidId_ReturnsBericht()
    {
        // Arrange
        var client = _factory.CreateClient();
        await LoescheAlleInventuren(client);
        // Erst alle Berichte abrufen, um eine gültige ID zu bekommen
        var berichteResponse = await client.GetAsync("/inventur/berichte");
        berichteResponse.EnsureSuccessStatusCode();
        var berichte = await berichteResponse.Content.ReadFromJsonAsync<List<InventurBerichtDto>>();
        
        // Wenn keine Berichte vorhanden sind, erstellen wir einen durch einen vollständigen Inventurprozess
        if (berichte == null || berichte.Count == 0)
        {
            // Vollständigen Prozess durchführen (wie im Test oben)
            var createRequest = new CreateInventurRequest
            {
                Bezeichnung = "Inventur für Berichts-ID-Test " + DateTime.Now.Ticks,
                Bemerkung = "Berichts-ID-Abruf wird getestet",
                ErstelltVon = "prodbysmolec"
            };
            
            var createResponse = await client.PostAsJsonAsync("/inventur", createRequest);
            createResponse.EnsureSuccessStatusCode();
            var inventur = await createResponse.Content.ReadFromJsonAsync<InventurDto>();
            
            var startResponse = await client.PostAsync($"/inventur/{inventur?.Id}/starten", null);
            startResponse.EnsureSuccessStatusCode();
            var startedInventur = await startResponse.Content.ReadFromJsonAsync<InventurDto>();
            
            foreach (var position in startedInventur!.Positionen)
            {
                var updateRequest = new UpdateInventurPositionRequest
                {
                    PositionId = position.Id,
                    ArtikelId = position.ArtikelId,
                    InventurID = startedInventur.Id,
                    GezaehlteMenge = position.SystemMenge,
                    Bemerkung = "Position für Berichts-ID-Test",
                    BearbeitetVon = "prodbysmolec",
                    IstGeprueft = true
                };
                
                await client.PutAsJsonAsync("/inventur/positionen", updateRequest);
            }
            
            await client.PostAsync($"/inventur/{inventur?.Id}/abschliessen", null);
            
            // Erneut alle Berichte abrufen
            berichteResponse = await client.GetAsync("/inventur/berichte");
            berichteResponse.EnsureSuccessStatusCode();
            berichte = await berichteResponse.Content.ReadFromJsonAsync<List<InventurBerichtDto>>();
        }
        
        Assert.NotNull(berichte);
        Assert.NotEmpty(berichte);
        
        var berichtId = berichte[0].Id;
        
        // Act
        var response = await client.GetAsync($"/inventur/berichte/{berichtId}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var bericht = await response.Content.ReadFromJsonAsync<InventurBerichtDto>();
        Assert.NotNull(bericht);
        Assert.Equal(berichtId, bericht.Id);
        Assert.NotEmpty(bericht.Inhalt);
    }

    [Fact]
    public async Task GetBerichtById_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();
        var invalidId = int.MaxValue; // Diese ID existiert höchstwahrscheinlich nicht
        
        // Act
        var response = await client.GetAsync($"/inventur/berichte/{invalidId}");
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetInventurBericht_ForNonExistingInventur_ReturnsNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();
        var nonExistingId = int.MaxValue; // Diese ID existiert höchstwahrscheinlich nicht
        
        // Act
        var response = await client.GetAsync($"/inventur/{nonExistingId}/bericht");
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private async static Task LoescheAlleInventuren(HttpClient client)
    {
        // 1. Alle Inventuren löschen.
        var response = await client.GetAsync("/inventur");
        var inventuren = await response.Content.ReadFromJsonAsync<List<InventurDto>>();
        // Verwende eine foreach-Schleife statt .ForEach()
        foreach (var inventur in inventuren!)
        {
            try
            {
                var url = $"Inventur/{inventur.Id}";
                var responseDelete = await client.DeleteAsync(url);

                if (responseDelete.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Inventur mit ID {inventur.Id} wurde erfolgreich gelöscht.");
                }
                else
                {
                    Console.WriteLine($"Fehler beim Löschen der Inventur mit ID {inventur.Id}. Statuscode: {responseDelete.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Löschen der Inventur mit ID {inventur.Id}: {ex.Message}");
            }
        }

    }

    #endregion
}