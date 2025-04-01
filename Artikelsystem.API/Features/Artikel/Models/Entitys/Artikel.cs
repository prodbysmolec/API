using System.ComponentModel.DataAnnotations.Schema;
using Artikelsystem.Api.Features.Employees.Enums;
using Artikelsystem.Api.Features.Wareneingang.Models.Entitys;
using Artikelsystem.Api.Models;
using Artikelsystem.Api.Features.Lieferant.Models.Entitys;
using Artikelsystem.Api.Features.Warenausgang.Models.DTOs.Responses;
using Artikelsystem.Api.Features.Warenausgang.Models.Entitys;
namespace Artikelsystem.Api.Features.Artikel.Models.Entitys;
    public class Artikel : AuditableEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Preis  { get; set; }
        public required int Maximalbestand { get; set; }
        public required int Mindestbestand { get; set; }
        public int Menge { get; set; }
        public ArtikelStatus Status { get; set; } = ArtikelStatus.Verfügbar;    
        public byte[] Bild { get; set; } = new byte[0];
        public virtual ICollection<WareneingangArtikelPositionen> Wareneingaenge { get; set; } = new List<WareneingangArtikelPositionen>();
        public virtual ICollection<WarenausgangArtikelPositionen> Warenausgaenge { get; set; } = new List<WarenausgangArtikelPositionen>();
        public virtual ArtikelStatistik? ArtikelStatistik { get; set; }
    }