using System;

namespace Artikelsystem.Api.Features.Wareneingang.Models.DTOs.Requests;

    public class WareneingangArtikelPositionenDto
    {
        public int Id { get; set; }
        public int ArtikelId { get; set; }
        public int WareneingangId { get; set; }
        public int Menge { get; set; }
        public decimal Einzelpreis { get; set; }
        public decimal Gesamtpreis { get; set; }
    }
