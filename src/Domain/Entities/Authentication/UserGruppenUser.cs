using System;

namespace Domain.Entities.Authentication;

public class UserGruppenUser
{
    public int UserID { get; set; }
    public User User { get; set; } = null!;
    public int UserGruppenID { get; set;}
    public UserGruppen UserGruppen { get; set; } = null!;
}
