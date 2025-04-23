using System;
using System.Globalization;
using Artikelsystem.Shared.DTOs.Artikel.Enums;
using Artikelsystem.Shared.DTOs.Artikel.Response;
using Avalonia.Data.Converters;

namespace Client.Avalonia.Converters;

public class BestandVerfuegbarConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ArtikelDto artikel)
        {
            // Artikel ist verfügbar, wenn Menge > Mindestbestand
            return artikel.Menge > artikel.Mindestbestand && artikel.Status != ArtikelStatus.UnterMindestbestand;
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}