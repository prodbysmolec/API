using System;

namespace Domain.Entities.Authentication;

public class Permission
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Beschreibung { get; set; }
    public required string Code { get; set; } // Eindeutiger Code zur Verwendung im Code
    
    // Navigation Property
    public virtual ICollection<GroupPermission> GroupPermissions { get; set; } = new List<GroupPermission>();
}
