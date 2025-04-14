using System;

namespace Artikelsystem.Shared.DTOs.ArtikelGruppe.Request;

public class GetAllArtikelGruppeResponse
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? ProduktKategorieId { get; set; }
}
