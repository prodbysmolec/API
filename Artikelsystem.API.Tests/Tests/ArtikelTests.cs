using System.Net;
using System.Net.Http.Json;
using Artikelsystem.Api.Features.Artikel.Models.DTOs;
using Artikelsystem.Api.Features.Employees.Enums;
using Artikelsystem.API.Tests;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Artikelsystem.Api.Tests.Tests;

public class ArtikelTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ArtikelTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAllArtikel_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/artikel");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAllArtikel_WithPagination_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/artikel?page=2&recordsPerPage=10");

        // Assert
        response.EnsureSuccessStatusCode();
        var artikels = await response.Content.ReadFromJsonAsync<List<GetArtikelResponse>>();
        Assert.NotNull(artikels);
        Assert.True(artikels.Count <= 10);
    }

    [Fact]
    public async Task GetAllArtikel_WithNameFilter_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var searchTerm = "Test";

        // Act
        var response = await client.GetAsync($"/artikel?nameContains={searchTerm}");

        // Assert
        response.EnsureSuccessStatusCode();
        var artikels = await response.Content.ReadFromJsonAsync<List<GetArtikelResponse>>();
        Assert.NotNull(artikels);
        foreach (var artikel in artikels)
        {
            Assert.Contains(searchTerm, artikel.Name, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public async Task GetAllArtikel_WithPriceRange_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var minPreis = 10.0m;
        var maxPreis = 100.0m;

        // Act
        var response = await client.GetAsync($"/artikel?minPreis={minPreis}&maxPreis={maxPreis}");

        // Assert
        response.EnsureSuccessStatusCode();
        var artikels = await response.Content.ReadFromJsonAsync<List<GetArtikelResponse>>();
        Assert.NotNull(artikels);
        foreach (var artikel in artikels)
        {
            Assert.True(artikel.Preis >= minPreis && artikel.Preis <= maxPreis);
        }
    }

    [Fact]
    public async Task GetAllArtikel_WithQuantityRange_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var minMenge = 5;
        var maxMenge = 50;

        // Act
        var response = await client.GetAsync($"/artikel?minMenge={minMenge}&maxMenge={maxMenge}");

        // Assert
        response.EnsureSuccessStatusCode();
        var artikels = await response.Content.ReadFromJsonAsync<List<GetArtikelResponse>>();
        Assert.NotNull(artikels);
        foreach (var artikel in artikels)
        {
            Assert.True(artikel.Menge >= minMenge && artikel.Menge <= maxMenge);
        }
    }

    [Fact]
    public async Task GetAllArtikel_WithStatusFilter_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var statusId = (int)ArtikelStatus.Verfügbar;

        // Act
        var response = await client.GetAsync($"/artikel?statusId={statusId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var artikels = await response.Content.ReadFromJsonAsync<List<GetArtikelResponse>>();
        Assert.NotNull(artikels);
        foreach (var artikel in artikels)
        {
            Assert.Equal(ArtikelStatus.Verfügbar, artikel.Status);
        }
    }

    [Fact]
    public async Task GetAllArtikel_UnterMindestbestand_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/artikel?unterMindestbestand=true");

        // Assert
        response.EnsureSuccessStatusCode();
        var artikels = await response.Content.ReadFromJsonAsync<List<GetArtikelResponse>>();
        Assert.NotNull(artikels);
        foreach (var artikel in artikels)
        {
            Assert.True(artikel.Menge < artikel.Mindestbestand);
        }
    }

    [Fact]
    public async Task GetAllArtikel_UeberMaximalbestand_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/artikel?ueberMaximalbestand=true");

        // Assert
        response.EnsureSuccessStatusCode();
        var artikels = await response.Content.ReadFromJsonAsync<List<GetArtikelResponse>>();
        Assert.NotNull(artikels);
        foreach (var artikel in artikels)
        {
            Assert.True(artikel.Menge > artikel.Maximalbestand);
        }
    }

    [Fact]
    public async Task GetAllArtikel_WithStatisticFilters_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var minLagerwert = 1000.0m;

        // Act
        var response = await client.GetAsync($"/artikel?minLagerwert={minLagerwert}");

        // Assert
        response.EnsureSuccessStatusCode();
        var artikels = await response.Content.ReadFromJsonAsync<List<GetArtikelResponse>>();
        Assert.NotNull(artikels);
        foreach (var artikel in artikels)
        {
            if (artikel.Statistik != null)
            {
                Assert.True(artikel.Statistik.Lagerwert >= minLagerwert);
            }
        }
    }

    [Fact]
    public async Task GetAllArtikel_WithSorting_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/artikel?sortBy=preis&sortDesc=true");

        // Assert
        response.EnsureSuccessStatusCode();
        var artikels = await response.Content.ReadFromJsonAsync<List<GetArtikelResponse>>();
        Assert.NotNull(artikels);
        
        for (int i = 0; i < artikels.Count - 1; i++)
        {
            Assert.True(artikels[i].Preis >= artikels[i + 1].Preis);
        }
    }

    [Fact]
    public async Task GetAllArtikel_WithMultipleFilters_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var nameContains = "Test";
        var minPreis = 20.0m;
        var maxPreis = 200.0m;
        var statusId = (int)ArtikelStatus.Verfügbar;

        // Act
        var response = await client.GetAsync($"/artikel?nameContains={nameContains}&minPreis={minPreis}&maxPreis={maxPreis}&statusId={statusId}&page=1&recordsPerPage=10");

        // Assert
        response.EnsureSuccessStatusCode();
        var artikels = await response.Content.ReadFromJsonAsync<List<GetArtikelResponse>>();
        Assert.NotNull(artikels);
        Assert.True(artikels.Count <= 10);
        
        foreach (var artikel in artikels)
        {
            Assert.Contains(nameContains, artikel.Name, StringComparison.OrdinalIgnoreCase);
            Assert.True(artikel.Preis >= minPreis && artikel.Preis <= maxPreis);
            Assert.Equal(ArtikelStatus.Verfügbar, artikel.Status);
        }
    }

    [Fact]
    public async Task GetAllArtikel_WithInvalidParameters_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/artikel?page=0&recordsPerPage=-5"); // Negative or zero values are invalid

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetAllArtikel_WithDurchschnittlicherEinzelpreisRange_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var minDurchschnittlicherEinzelpreis = 15.0m;
        var maxDurchschnittlicherEinzelpreis = 150.0m;

        // Act
        var response = await client.GetAsync($"/artikel?minDurchschnittlicherEinzelpreis={minDurchschnittlicherEinzelpreis}&maxDurchschnittlicherEinzelpreis={maxDurchschnittlicherEinzelpreis}");

        // Assert
        response.EnsureSuccessStatusCode();
        var artikels = await response.Content.ReadFromJsonAsync<List<GetArtikelResponse>>();
        Assert.NotNull(artikels);
        foreach (var artikel in artikels)
        {
            if (artikel.Statistik != null)
            {
                Assert.True(artikel.Statistik.DurchschnittlicherEinzelpreis >= minDurchschnittlicherEinzelpreis && 
                            artikel.Statistik.DurchschnittlicherEinzelpreis <= maxDurchschnittlicherEinzelpreis);
            }
        }
    }

    [Fact]
    public async Task GetAllArtikel_SortByName_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/artikel?sortBy=name&sortDesc=false");

        // Assert
        response.EnsureSuccessStatusCode();
        var artikels = await response.Content.ReadFromJsonAsync<List<GetArtikelResponse>>();
        Assert.NotNull(artikels);
        
        for (int i = 0; i < artikels.Count - 1; i++)
        {
            Assert.True(string.Compare(artikels[i].Name, artikels[i + 1].Name, StringComparison.OrdinalIgnoreCase) <= 0);
        }
    }

    [Fact]
    public async Task GetAllArtikel_SortByMenge_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/artikel?sortBy=menge&sortDesc=false");

        // Assert
        response.EnsureSuccessStatusCode();
        var artikels = await response.Content.ReadFromJsonAsync<List<GetArtikelResponse>>();
        Assert.NotNull(artikels);
        
        for (int i = 0; i < artikels.Count - 1; i++)
        {
            Assert.True(artikels[i].Menge <= artikels[i + 1].Menge);
        }
    }

    [Fact]
    public async Task GetAllArtikel_SortByLagerwert_ReturnsOkResult()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/artikel?sortBy=lagerwert&sortDesc=true");

        // Assert
        response.EnsureSuccessStatusCode();
        var artikels = await response.Content.ReadFromJsonAsync<List<GetArtikelResponse>>();
        Assert.NotNull(artikels);
        
        decimal? GetLagerwert(GetArtikelResponse artikel) => artikel.Statistik?.Lagerwert ?? 0;
        
        for (int i = 0; i < artikels.Count - 1; i++)
        {
            Assert.True(GetLagerwert(artikels[i]) >= GetLagerwert(artikels[i + 1]));
        }
    }
}