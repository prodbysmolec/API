using System;
using System.ComponentModel.DataAnnotations.Schema;
using Artikelsystem.Api.Models;

namespace Artikelsystem.Api.Features.Artikel.Models.Entitys;

public class ArtikelStatistik : AuditableEntity
{
    public int Id { get; set; }
    public int ArtikelId { get; set; }
    
    public decimal Gesamtmenge { get; set; }
    public decimal DurchschnittlicherEinzelpreis { get; set; }
    public decimal DurchschnittlicherVerkaufspreis { get; set; }
    public int VerkaufsMenge { get; set; }
    
    // Berechnete Eigenschaften

    // Lagerwert = GesamtMenge * DurchschnittlicherEinzelpreis
    private decimal _Lagerwert;
    public decimal Lagerwert
    {
        get { return _Lagerwert; }
        set { _Lagerwert = value; }
    }
    
    // GesamtVerkaufswert = VerkaufsMeneg * DurchschnittlicherVerkaufspreis 
    private decimal _GesamtVerkaufswert;
    public decimal GesamtVerkaufswert
    {
        get { return _GesamtVerkaufswert; }
        set { _GesamtVerkaufswert = value; }
    }
    
    
    // Navigation property
    public virtual Artikel? Artikel { get; set; }
}