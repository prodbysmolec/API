using System;

namespace Domain.Entities.Authentication;

public class User
{

    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public required string PasswordHash { get; set; } = null!;  
    public required string Name { get; set; } = null!;
    public required string Nachname { get; set; } = null!;
    public required string Email { get; set; } = null!;
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public ICollection<UserGruppenUser> UserGruppenUsers { get; set; } = new List<UserGruppenUser>();

}
