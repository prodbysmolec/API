using System;

namespace Domain.Entities.Authentication;

public class GroupPermission
{
    public int UserGruppenID { get; set; }
    public UserGruppen UserGruppen { get; set; } = null!;
    public int PermissionID { get; set; }
    public Permission Permission { get; set; } = null!;
}