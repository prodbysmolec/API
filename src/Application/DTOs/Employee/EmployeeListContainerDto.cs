using System;

namespace Application.DTOs.Employee;

public class EmployeeListContainerDto
{
    public List<EmployeeListDto> Employees { get; set; } = new ();

    /// <summary>
    /// Die aktuelle Seite der Ergebnisse.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Die Anzahl der Datensätze pro Seite.
    /// </summary>
    public int RecordsPerPage { get; set; }

    /// <summary>
    /// Die Gesamtanzahl der Datensätze.
    /// </summary>
    public int TotalRecords { get; set; }

    /// <summary>
    /// Die Gesamtanzahl der Seiten.
    /// </summary>
    public int TotalPages { get; set; }

}
