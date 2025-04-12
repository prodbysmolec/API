using System;

namespace Domain.Entities.Authentication;

public class UserGruppen
{
    public int Id { get; set; }
    public required string Name { get; set; }

    // Navigation Property
    public virtual ICollection<UserGruppenUser> UserGruppenUsers { get; set; } = new List<UserGruppenUser>();
}
