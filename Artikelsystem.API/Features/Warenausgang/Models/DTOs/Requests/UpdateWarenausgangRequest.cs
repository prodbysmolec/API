using System;

namespace Artikelsystem.Api.Features.Warenausgang.Models.DTOs.Requests;

public class UpdateWarenausgangRequest
{
    public int Id { get; set; } 
    public string AllgemeineBemerkungen { get; set; } = "";
    public List<UpdateWarenausgangRequest> ArtikelPositionen { get; set; } = new List<UpdateWarenausgangRequest>();
}
