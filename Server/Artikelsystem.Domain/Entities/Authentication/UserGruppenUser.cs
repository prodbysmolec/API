using System;

namespace Artikelsystem.API.Features.Authentication.Models.Entitys;

public class UserGruppenUser
{
    public int UserID { get; set; }
    public User User { get; set; } = null!;
    public int UserGruppenID { get; set;}
    public UserGruppen UserGruppen { get; set; } = null!;
}
