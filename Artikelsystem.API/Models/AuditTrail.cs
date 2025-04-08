using System;

namespace Artikelsystem.Api.Models;

public abstract class AuditableEntity
{
    public string? ErstelltVon { get; set; }
    public DateTime ErstelltAm { get; set; }
    public string? BearbeitetVon { get; set; }
    public DateTime BearbeitetAm { get; set; }
}
