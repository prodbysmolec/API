using System;

namespace Artikelsystem.Api.Features.Warenausgang.Models.DTOs.Responses;

public class WarenausgangDto
{
        public int Id { get; set; }
        public string Mitarbeiter { get; set; } = "";
        public string AllgemeineBemerkungen { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; } = "";
        public string UpdatedBy { get; set; } = "";
        public ICollection<WarenausgangArtikelPositionDto> ArtikelPositionen { get; set; } = new List<WarenausgangArtikelPositionDto>();
}
