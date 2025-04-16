using System;

namespace Domain.Common;

public interface IAuditable
{
    string? ErstelltVon { get; set; }
    DateTime ErstelltAm { get; set; }
    string? BearbeitetVon { get; set; }
    DateTime BearbeitetAm { get; set; }
}
