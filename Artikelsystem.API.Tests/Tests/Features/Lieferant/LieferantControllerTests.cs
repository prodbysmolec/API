using System;
using System.Net;
using System.Net.Http.Json;
using Artikelsystem.Api.Features.Lieferant.Models.DTOs;
using Artikelsystem.API.Tests;

namespace Artikelsystem.Api.Tests.Tests.Features.Lieferant;

public class LieferantControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public LieferantControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAllLieferanten_ReturnsSuccessResult()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/lieferanten");

        // Assert
        response.EnsureSuccessStatusCode();
        var lieferanten = await response.Content.ReadFromJsonAsync<List<LieferantDto>>();
        Assert.NotNull(lieferanten);
    }

    // [Fact]
    // public async Task GetLieferantById_ExistingId_ReturnsOkResult()
    // {
    //     // Arrange
    //     var client = _factory.CreateClient();

    //     // Erst einen neuen Lieferanten erstellen, um eine bekannte ID zu haben
    //     var createRequest = new CreateLieferantRequest
    //     {
    //         Firma = "Test Firma für GetById",
    //         Name = "Tester",
    //         Vorname = "Thomas",
    //         EmailAdresse = "thomas.tester@test.de",
    //         Strasse = "Teststraße",
    //         Hausnummer = "1",
    //         PLZ = "12345",
    //         Ort = "Teststadt",
    //         Telefonnummer = "01234-56789"
    //     };

    //     var createResponse = await client.PostAsJsonAsync("/api/lieferanten", createRequest);
    //     createResponse.EnsureSuccessStatusCode();
    //     var createdLieferant = await createResponse.Content.ReadFromJsonAsync<LieferantDto>();
    //     Assert.NotNull(createdLieferant);

    //     // Act
    //     var response = await client.GetAsync($"/api/lieferanten/{createdLieferant.Id}");

    //     // Assert
    //     response.EnsureSuccessStatusCode();
    //     var lieferant = await response.Content.ReadFromJsonAsync<LieferantDetailDto>();
    //     Assert.NotNull(lieferant);
    //     Assert.Equal(createdLieferant.Id, lieferant.Id);
    //     Assert.Equal(createRequest.Firma, lieferant.Firma);
    // }

    // [Fact]
    // public async Task GetLieferantById_NonExistingId_ReturnsNotFound()
    // {
    //     // Arrange
    //     var client = _factory.CreateClient();
    //     var nonExistingId = int.MaxValue; // Diese ID existiert mit hoher Wahrscheinlichkeit nicht

    //     // Act
    //     var response = await client.GetAsync($"/api/lieferanten/{nonExistingId}");

    //     // Assert
    //     Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    // }

    // [Fact]
    // public async Task CreateLieferant_ValidRequest_ReturnsCreatedResult()
    // {
    //     // Arrange
    //     var client = _factory.CreateClient();
    //     var request = new CreateLieferantRequest
    //     {
    //         Firma = "Neue Test-Firma " + DateTime.Now.Ticks,
    //         Name = "Ersteller",
    //         Vorname = "Ernst",
    //         EmailAdresse = "ernst.ersteller@test.de",
    //         Strasse = "Erstellungsweg",
    //         Hausnummer = "5",
    //         PLZ = "54321",
    //         Ort = "Teststadt",
    //         Telefonnummer = "09876-54321"
    //     };

    //     // Act
    //     var response = await client.PostAsJsonAsync("/api/lieferanten", request);

    //     // Assert
    //     Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    //     var lieferant = await response.Content.ReadFromJsonAsync<LieferantDto>();
    //     Assert.NotNull(lieferant);
    //     Assert.Equal(request.Firma, lieferant.Firma);
    //     Assert.Equal(request.Name, lieferant.Name);
    //     Assert.True(lieferant.IstAktiv);
    // }

    // [Fact]
    // public async Task UpdateLieferant_ExistingIdWithValidData_ReturnsOkResult()
    // {
    //     // Arrange
    //     var client = _factory.CreateClient();

    //     // Erst einen neuen Lieferanten erstellen
    //     var createRequest = new CreateLieferantRequest
    //     {
    //         Firma = "Update Test-Firma " + DateTime.Now.Ticks,
    //         Name = "Update",
    //         Vorname = "Udo",
    //         EmailAdresse = "udo.update@test.de",
    //         Strasse = "Updateweg",
    //         Hausnummer = "10",
    //         PLZ = "67890",
    //         Ort = "Updatestadt",
    //         Telefonnummer = "01111-22222"
    //     };

    //     var createResponse = await client.PostAsJsonAsync("/api/lieferanten", createRequest);
    //     createResponse.EnsureSuccessStatusCode();
    //     var createdLieferant = await createResponse.Content.ReadFromJsonAsync<LieferantDto>();
    //     Assert.NotNull(createdLieferant);

    //     // Update Request vorbereiten
    //     var updateRequest = new UpdateLieferantRequest
    //     {
    //         Firma = "Aktualisierte Firma",
    //         Name = createRequest.Name,
    //         Vorname = createRequest.Vorname,
    //         EmailAdresse = "neu@aktualisiert.de",
    //         Strasse = createRequest.Strasse,
    //         Hausnummer = createRequest.Hausnummer,
    //         PLZ = createRequest.PLZ,
    //         Ort = createRequest.Ort,
    //         Telefonnummer = createRequest.Telefonnummer,
    //         IstAktiv = true
    //     };

    //     // Act
    //     var response = await client.PutAsJsonAsync($"/api/lieferanten/{createdLieferant.Id}", updateRequest);

    //     // Assert
    //     response.EnsureSuccessStatusCode();
    //     var updatedLieferant = await response.Content.ReadFromJsonAsync<LieferantDto>();
    //     Assert.NotNull(updatedLieferant);
    //     Assert.Equal(createdLieferant.Id, updatedLieferant.Id);
    //     Assert.Equal("Aktualisierte Firma", updatedLieferant.Firma);
    //     Assert.Equal("neu@aktualisiert.de", updatedLieferant.EmailAdresse);
    // }

    // [Fact]
    // public async Task DeactivateLieferant_ExistingId_ReturnsOkResult()
    // {
    //     // Arrange
    //     var client = _factory.CreateClient();

    //     // Erst einen neuen Lieferanten erstellen
    //     var createRequest = new CreateLieferantRequest
    //     {
    //         Firma = "Deaktivierungsfirma " + DateTime.Now.Ticks,
    //         Name = "Deaktivierung",
    //         Vorname = "Dieter",
    //         EmailAdresse = "dieter.deaktivierung@test.de",
    //         Strasse = "Deaktivierungsstraße",
    //         Hausnummer = "15",
    //         PLZ = "11111",
    //         Ort = "Deaktivierungsstadt",
    //         Telefonnummer = "02222-33333"
    //     };

    //     var createResponse = await client.PostAsJsonAsync("/api/lieferanten", createRequest);
    //     createResponse.EnsureSuccessStatusCode();
    //     var createdLieferant = await createResponse.Content.ReadFromJsonAsync<LieferantDto>();
    //     Assert.NotNull(createdLieferant);

    //     // Act
    //     var response = await client.PatchAsync($"/api/lieferanten/{createdLieferant.Id}/deactivate", null);

    //     // Assert
    //     response.EnsureSuccessStatusCode();

    //     // Überprüfen, ob der Lieferant deaktiviert wurde
    //     var getLieferantResponse = await client.GetAsync($"/api/lieferanten/{createdLieferant.Id}");
    //     getLieferantResponse.EnsureSuccessStatusCode();
    //     var lieferant = await getLieferantResponse.Content.ReadFromJsonAsync<LieferantDetailDto>();
    //     Assert.NotNull(lieferant);
    //     Assert.False(lieferant.IstAktiv);
    // }

    // [Fact]
    // public async Task DeleteLieferant_ExistingId_ReturnsOkResult()
    // {
    //     // Arrange
    //     var client = _factory.CreateClient();

    //     // Erst einen neuen Lieferanten erstellen
    //     var createRequest = new CreateLieferantRequest
    //     {
    //         Firma = "Löschfirma " + DateTime.Now.Ticks,
    //         Name = "Löscher",
    //         Vorname = "Lars",
    //         EmailAdresse = "lars.loescher@test.de",
    //         Strasse = "Löschweg",
    //         Hausnummer = "20",
    //         PLZ = "22222",
    //         Ort = "Löschstadt",
    //         Telefonnummer = "03333-44444"
    //     };

    //     var createResponse = await client.PostAsJsonAsync("/api/lieferanten", createRequest);
    //     createResponse.EnsureSuccessStatusCode();
    //     var createdLieferant = await createResponse.Content.ReadFromJsonAsync<LieferantDto>();
    //     Assert.NotNull(createdLieferant);

    //     // Act
    //     var response = await client.DeleteAsync($"/api/lieferanten/{createdLieferant.Id}");

    //     // Assert
    //     response.EnsureSuccessStatusCode();

    //     // Überprüfen, ob der Lieferant gelöscht wurde
    //     var getLieferantResponse = await client.GetAsync($"/api/lieferanten/{createdLieferant.Id}");
    //     Assert.Equal(HttpStatusCode.NotFound, getLieferantResponse.StatusCode);
    // }

    // [Fact]
    // public async Task SearchLieferanten_WithValidTerm_ReturnsMatchingResults()
    // {
    //     // Arrange
    //     var client = _factory.CreateClient();

    //     // Erst einen neuen Lieferanten mit eindeutigem Namen erstellen
    //     var uniqueName = "Uniquetest" + DateTime.Now.Ticks;
    //     var createRequest = new CreateLieferantRequest
    //     {
    //         Firma = "Suchfirma",
    //         Name = uniqueName,
    //         Vorname = "Samuel",
    //         EmailAdresse = "samuel.sucher@test.de",
    //         Strasse = "Suchweg",
    //         Hausnummer = "25",
    //         PLZ = "33333",
    //         Ort = "Suchstadt",
    //         Telefonnummer = "04444-55555"
    //     };

    //     var createResponse = await client.PostAsJsonAsync("/api/lieferanten", createRequest);
    //     createResponse.EnsureSuccessStatusCode();

    //     // Act
    //     var response = await client.GetAsync($"/api/lieferanten/search?suchbegriff={uniqueName}");

    //     // Assert
    //     response.EnsureSuccessStatusCode();
    //     var lieferanten = await response.Content.ReadFromJsonAsync<List<LieferantDto>>();
    //     Assert.NotNull(lieferanten);
    //     Assert.Single(lieferanten);
    //     Assert.Equal(uniqueName, lieferanten[0].Name);
    // }

    // [Fact]
    // public async Task GetAllLieferanten_NurAktive_ReturnsOnlyActiveLieferanten()
    // {
    //     // Arrange
    //     var client = _factory.CreateClient();

    //     // 1. Aktiven Lieferanten erstellen
    //     var aktivRequest = new CreateLieferantRequest
    //     {
    //         Firma = "Aktiv Firma " + DateTime.Now.Ticks,
    //         Name = "Aktiv",
    //         Vorname = "Anton",
    //         EmailAdresse = "anton.aktiv@test.de",
    //         Strasse = "Aktivstraße",
    //         Hausnummer = "1",
    //         PLZ = "44444",
    //         Ort = "Aktivstadt",
    //         Telefonnummer = "05555-66666"
    //     };

    //     await client.PostAsJsonAsync("/api/lieferanten", aktivRequest);

    //     // 2. Lieferanten erstellen und deaktivieren
    //     var inaktivRequest = new CreateLieferantRequest
    //     {
    //         Firma = "Inaktiv Firma " + DateTime.Now.Ticks,
    //         Name = "Inaktiv",
    //         Vorname = "Ingo",
    //         EmailAdresse = "ingo.inaktiv@test.de",
    //         Strasse = "Inaktivstraße",
    //         Hausnummer = "2",
    //         PLZ = "55555",
    //         Ort = "Inaktivstadt",
    //         Telefonnummer = "06666-77777"
    //     };

    //     var inaktivResponse = await client.PostAsJsonAsync("/api/lieferanten", inaktivRequest);
    //     inaktivResponse.EnsureSuccessStatusCode();
    //     var inaktivLieferant = await inaktivResponse.Content.ReadFromJsonAsync<LieferantDto>();

    //     // Deaktivieren
    //     await client.PatchAsync($"/api/lieferanten/{inaktivLieferant?.Id}/deactivate", null);

    //     // Act
    //     var response = await client.GetAsync("/api/lieferanten?nurAktive=true");

    //     // Assert
    //     response.EnsureSuccessStatusCode();
    //     var lieferanten = await response.Content.ReadFromJsonAsync<List<LieferantDto>>();
    //     Assert.NotNull(lieferanten);
    //     Assert.All(lieferanten, l => Assert.True(l.IstAktiv));
    //     Assert.DoesNotContain(lieferanten, l => l.Id == inaktivLieferant?.Id);
    // }
}
