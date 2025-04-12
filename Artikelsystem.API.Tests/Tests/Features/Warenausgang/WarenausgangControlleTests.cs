using System;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Domain.Entities.Inventur;
using Domain.Entities.Warenausgang;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Request;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;

namespace API.Tests.Tests.Features.Warenausgang;

public class WarenausgangControllerTests : IClassFixture<CustomWebApplicationFactory>
{ 
    private readonly CustomWebApplicationFactory _factory;

    public WarenausgangControllerTests()
    {
        _factory = new CustomWebApplicationFactory();
    }

    [Fact]    
    public async Task GetAllWarenausgaenge_ReturnsSuccessResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = "/Warenausgang";

        // Act
        var response = await client.GetAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetWarenausgangById_ReturnsSuccessResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = "/Warenausgang/1";

        // Act
        var response = await client.GetAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateWarenausgangAsync_ValidRequest_ReturnsCreatedResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = "/Warenausgang";
        await _factory.DeleteAllEntities<Inventur>(client, "/Inventur");

        var createDto = new WarenausgangRequestDto
        {
            AllgemeineBemerkungen = "Test Bemerkung",
            Zweck = Artikelsystem.Shared.DTOs.Warenausgang.Enums.WarenausgangZweckEnum.Bestellung,
            ArtikelPositionen = new List<CreateWarenausgangArtikelPositionDto>
            {
                new CreateWarenausgangArtikelPositionDto
                {
                    ArtikelId = 1,
                    Menge = 10,
                    Bemerkung = "Test Artikel",
                    Verkaufspreis = 100.00m,
                    Rechnungsnummer = "123456"
                }
            }
        };

        // Act
        var response = await client.PostAsJsonAsync(request, createDto);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(responseContent), "Response content is null or empty.");

        var createdWarenausgang = JsonSerializer.Deserialize<WarenausgangDto>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(createdWarenausgang);
        Assert.Equal(createDto.AllgemeineBemerkungen, createdWarenausgang.AllgemeineBemerkungen);
        Assert.Equal(createDto.Zweck, createdWarenausgang.Zweck);
        Assert.NotNull(createdWarenausgang.ArtikelPositionen);
        Assert.Single(createdWarenausgang.ArtikelPositionen);
        Assert.Equal(13, createdWarenausgang.ArtikelPositionen.First().ArtikelId);
        Assert.Equal(createDto.ArtikelPositionen.First().Menge, createdWarenausgang.ArtikelPositionen.First().Menge);
    }

    [Fact]
    public async Task CreateWarenausgangAsync_InvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = "/Warenausgang";
        await _factory.DeleteAllEntities<Inventur>(client, "/Inventur");

        var createDto = new WarenausgangRequestDto(); // Missing required fields

        // Act
        var response = await client.PostAsJsonAsync(request, createDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(responseContent), "Response content is null or empty.");
        Assert.Contains("Der Zweck des Warenausgangs darf nicht 'None' sein", responseContent);
    }


}

