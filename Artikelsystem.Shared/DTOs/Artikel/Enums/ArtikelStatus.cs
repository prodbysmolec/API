using System;
using System.ComponentModel;

namespace Artikelsystem.Shared.DTOs.Artikel.Enums;

public enum ArtikelStatus
{
    [Description("Verfügbar")]
    Verfügbar = 0,
    [Description("Unter Mindestbestand")]
    UnterMindestbestand = 1
}
