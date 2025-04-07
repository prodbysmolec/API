using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Artikelsystem.Api.Features.Artikel.Models.DTOs;
using Artikelsystem.Api.Features.Lieferant.Models.DTOs;
using Artikelsystem.API.Features.Lieferant.Models.DTOs.Request;
using Artikelsystem.Api.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Newtonsoft.Json;
using System.Text;
using Artikelsystem.Api.Features.Artikel.Models.Entitys;
using Artikelsystem.Api.Features.Lieferant.Controllers;
using Artikelsystem.API.Tests;

namespace Artikelsystem.Tests.Integration
{
    public class LieferantenArtikelIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {

        private readonly WebApplicationFactory<Program> _factory;

        public LieferantenArtikelIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        private void SeedTestDatabase(AppDbContext context)
        {
            // Seed with basic data if needed for the tests
            var testArtikel = new Artikel
            {
                Name = "Test Artikel",
                CreatedBy = "Test",
                CreatedOn = DateTime.Now,
                Maximalbestand = 20,
                Mindestbestand = 10,
            };

            context.Artikel.Add(testArtikel);
            context.SaveChanges();
        }

        [Fact]
        public async Task CreateLieferantAndArtikelLieferant_ShouldSucceed()
        {
            // Arrange - Create a new Lieferant 
            var client = _factory.CreateClient();
            await LoescheAlleLieferanten(client);
            var request = new CreateLieferantRequest
            {
                Firma = "Neue Test-Firma " + DateTime.Now.Ticks,
                Name = "Ersteller",
                Vorname = "Ernst",
                Strasse = "Erstellungsweg",
                Hausnummer = "5",
                PLZ = "54321",
                Ort = "Teststadt",
                Telefonnummer = "09876-54321",
                EmailAdresse = "ernst.ersteller@test.de",
            };

            // Act - POST the Lieferant
            var lieferantResponse = await client.PostAsJsonAsync("/Lieferanten", request);
            
            // Assert
            Assert.Equal(HttpStatusCode.Created, lieferantResponse.StatusCode);
            
            var lieferantContent = await lieferantResponse.Content.ReadFromJsonAsync<LieferantDto>();
            Assert.NotNull(lieferantContent);
            Assert.Equal(request.Firma, lieferantContent.Firma);
            
            // Get the first article from DB for test
            var artikelResponse = await client.GetAsync("/Artikel");
            artikelResponse.EnsureSuccessStatusCode();
            
            var artikelContent = await artikelResponse.Content.ReadFromJsonAsync<List<ArtikelDto>>();
            Assert.NotNull(artikelContent);
            Assert.NotEmpty(artikelContent);
            
            var testArtikelId = artikelContent[0].Id;
            var lieferantId = lieferantContent.Id;
            
            // Create ArtikelLieferant relationship
            var artikelLieferantRequest = new ArtikelLieferantAddDto
            {
                Einkaufspreis = 42.99m,
                IstPrimaer = true,
                Mindestbestellmenge = 10,
                Lieferzeit = 5,
                ArtikelNrBeimLieferanten = "SUP-001"
            };
            
            // Convert to JSON with proper serialization
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(artikelLieferantRequest),
                Encoding.UTF8,
                "application/json");
            
            // Act - Add Lieferant to Artikel
            var artikelLieferantResponse = await client.PostAsync(
                $"/ArtikelLieferant/artikel/{testArtikelId}/lieferanten/{lieferantId}", 
                jsonContent);
            
            // Assert
            Assert.Equal(HttpStatusCode.Created, artikelLieferantResponse.StatusCode);
            
            var artikelLieferantContent = await artikelLieferantResponse.Content.ReadFromJsonAsync<ArtikelLieferantDto>();
            Assert.NotNull(artikelLieferantContent);
            Assert.Equal(lieferantId, artikelLieferantContent.LieferantId);
            Assert.Equal(testArtikelId, artikelLieferantContent.ArtikelId);
            Assert.Equal(artikelLieferantRequest.Einkaufspreis, artikelLieferantContent.Einkaufspreis);
            Assert.Equal(artikelLieferantRequest.IstPrimaer, artikelLieferantContent.IstPrimaerLieferant);
            Assert.Equal(artikelLieferantRequest.ArtikelNrBeimLieferanten, artikelLieferantContent.ArtikelNrBeimLieferanten);
            
            // Verify that we can retrieve this relationship
            var getLieferantenResponse = await client.GetAsync($"/ArtikelLieferant/artikel/{testArtikelId}/lieferanten");
            getLieferantenResponse.EnsureSuccessStatusCode();
            
            var lieferanten = await getLieferantenResponse.Content.ReadFromJsonAsync<List<ArtikelLieferantDto>>();
            Assert.NotNull(lieferanten);
            Assert.NotEmpty(lieferanten);
            Assert.Contains(lieferanten, l => l.LieferantId == lieferantId && l.ArtikelId == testArtikelId);
            
            // Verify primary supplier selection
            var primaryResponse = await client.GetAsync($"/ArtikelLieferant/artikel/{testArtikelId}/lieferanten/primaer");
            primaryResponse.EnsureSuccessStatusCode();
            
            var primaryLieferant = await primaryResponse.Content.ReadFromJsonAsync<ArtikelLieferantDto>();
            Assert.NotNull(primaryLieferant);
            Assert.Equal(lieferantId, primaryLieferant.LieferantId);
            Assert.True(primaryLieferant.IstPrimaerLieferant);
        }

        [Fact]
        public async Task UpdateArtikelLieferant_ShouldSucceed()
        {
            // First create the initial data
            var client = _factory.CreateClient();
            await LoescheAlleLieferanten(client);
            await CreateLieferantAndArtikelLieferant_ShouldSucceed();
            
            // Get the existing artikel and lieferant to use for the update test
            var artikelResponse = await client.GetAsync("/Artikel");
            var artikelContent = await artikelResponse.Content.ReadFromJsonAsync<List<ArtikelDto>>();
            var testArtikelId = artikelContent[0].Id;
            
            var lieferantenResponse = await client.GetAsync("/Lieferanten");
            var lieferantenContent = await lieferantenResponse.Content.ReadFromJsonAsync<List<LieferantDto>>();
            var lieferantId = lieferantenContent[0].Id;
            
            // Update the ArtikelLieferant relationship
            var updateRequest = new ArtikelLieferantUpdateDto
            {
                Einkaufspreis = 39.99m,
                IstPrimaer = true,
                Mindestbestellmenge = 5,
                Lieferzeit = 3,
                ArtikelNrBeimLieferanten = "UPDATED-001"
            };
            
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(updateRequest),
                Encoding.UTF8,
                "application/json");
            
            // Act - Update the relationship
            var updateResponse = await client.PutAsync(
                $"/ArtikelLieferant/artikel/{testArtikelId}/lieferanten/{lieferantId}", 
                jsonContent);
            
            // Assert
            updateResponse.EnsureSuccessStatusCode();
            
            var updatedContent = await updateResponse.Content.ReadFromJsonAsync<ArtikelLieferantDto>();
            Assert.NotNull(updatedContent);
            Assert.Equal(updateRequest.Einkaufspreis, updatedContent.Einkaufspreis);
            Assert.Equal(updateRequest.ArtikelNrBeimLieferanten, updatedContent.ArtikelNrBeimLieferanten);
            
            // Verify the update through GET
            var getResponse = await client.GetAsync($"/ArtikelLieferant/artikel/{testArtikelId}/lieferanten");
            getResponse.EnsureSuccessStatusCode();
            
            var getContent = await getResponse.Content.ReadFromJsonAsync<List<ArtikelLieferantDto>>();
            var updated = getContent.FirstOrDefault(l => l.LieferantId == lieferantId);
            Assert.NotNull(updated);
            Assert.Equal(updateRequest.Einkaufspreis, updated.Einkaufspreis);
            Assert.Equal(updateRequest.ArtikelNrBeimLieferanten, updated.ArtikelNrBeimLieferanten);
        }

    [Fact]
    public async Task DeactivateAndDeleteArtikelLieferant_ShouldSucceed()
    {
        // First create the initial data
        var client = _factory.CreateClient();
        await CreateLieferantAndArtikelLieferant_ShouldSucceed();
        
        // Get the existing artikel and lieferant IDs
        var artikelResponse = await client.GetAsync("/Artikel");
        var artikelContent = await artikelResponse.Content.ReadFromJsonAsync<List<ArtikelDto>>();
        var testArtikelId = artikelContent[0].Id;
        
        var lieferantenResponse = await client.GetAsync("/Lieferanten");
        var lieferantenContent = await lieferantenResponse.Content.ReadFromJsonAsync<List<LieferantDto>>();
        var lieferantId = lieferantenContent[0].Id;
        
        // First deactivate the relationship
        var deactivateResponse = await client.PatchAsync(
            $"/ArtikelLieferant/artikel/{testArtikelId}/lieferanten/{lieferantId}/deactivate", 
            null);
        
        // Assert deactivation
        deactivateResponse.EnsureSuccessStatusCode();
        
        // Verify the relationship is deactivated in the database directly
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var relationship = context.ArtikelLieferanten
                .FirstOrDefault(al => al.ArtikelId == testArtikelId && al.LieferantId == lieferantId);
            
            Assert.NotNull(relationship); // Relationship should still exist
            Assert.False(relationship.IstAktiv); // But should be inactive
        }
        
        // Now delete the relationship
        var deleteResponse = await client.DeleteAsync($"/ArtikelLieferant/artikel/{testArtikelId}/lieferanten/{lieferantId}");
        
        // Assert deletion
        deleteResponse.EnsureSuccessStatusCode();
        
        // Verify it's completely gone
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var relationship = context.ArtikelLieferanten
                .FirstOrDefault(al => al.ArtikelId == testArtikelId && al.LieferantId == lieferantId);
            
            Assert.Null(relationship);
        }
    }

    private async static Task LoescheAlleLieferanten(HttpClient client)
    {
        try
        {
            // Get all existing Lieferanten
            var response = await client.GetAsync("/Lieferanten");
            
            if (response.IsSuccessStatusCode)
            {
                var lieferanten = await response.Content.ReadFromJsonAsync<List<LieferantDto>>();
                
                if (lieferanten != null && lieferanten.Any())
                {
                    // Delete each Lieferant one by one
                    foreach (var lieferant in lieferanten)
                    {
                        try
                        {
                            // First try to deactivate any Lieferant (to avoid constraint violations)
                            await client.PatchAsync($"/Lieferanten/{lieferant.Id}/deactivate", null);
                            
                            // Then delete the Lieferant
                            var deleteResponse = await client.DeleteAsync($"/Lieferanten/{lieferant.Id}");
                            
                            // Log error but continue if deletion fails
                            if (!deleteResponse.IsSuccessStatusCode)
                            {
                                Console.WriteLine($"Failed to delete Lieferant with ID {lieferant.Id}. Status: {deleteResponse.StatusCode}");
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log but continue with next item to ensure test can proceed
                            Console.WriteLine($"Error deleting Lieferant with ID {lieferant.Id}: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"Failed to retrieve Lieferanten list. Status: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            // Log but allow test to continue
            Console.WriteLine($"Error in LoescheAlleLieferanten: {ex.Message}");
        }
    }
    }


    // Define any DTOs needed for the test that might not be accessible or aren't included in your provided code
    public class ArtikelLieferantUpdateDto : ArtikelLieferantAddDto
    {
        // This inherits all properties from ArtikelLieferantAddDto
    }
}