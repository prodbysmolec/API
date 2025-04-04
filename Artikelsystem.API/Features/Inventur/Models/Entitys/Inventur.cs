using System;
using Artikelsystem.Api.Features.Inventur.Models.Enums;
using Artikelsystem.Api.Models;

namespace Artikelsystem.Api.Features.Inventur.Models.Entitys;

public class Inventur : AuditableEntity
{
    public int Id { get; set; }
    public string? Bezeichnung { get; set; } 
    public DateTime StartDatum { get; set; }
    public DateTime? AbschlussDatum { get; set; }
    public InventurStatus Status { get; set; }
    public string? Bemerkung { get; set; }
    public virtual ICollection<InventurPosition> Positionen { get; set; } = new HashSet<InventurPosition>();
    public virtual ICollection<InventurBerichte> Berichte { get; set; } = new HashSet<InventurBerichte>();
}
