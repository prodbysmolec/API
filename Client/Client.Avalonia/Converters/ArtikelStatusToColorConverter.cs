using System;
using System.Globalization;
using Artikelsystem.Shared.DTOs.Artikel.Enums;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Client.Avalonia.Converters;

public class ArtikelStatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ArtikelStatus status)
        {
            return status switch
            {
                ArtikelStatus.Verfügbar => new SolidColorBrush(Color.Parse("#4CAF50")),
                ArtikelStatus.UnterMindestbestand => new SolidColorBrush(Color.Parse("#FF9800")),
                _ => new SolidColorBrush(Color.Parse("#9E9E9E"))
            };
        }
            
        return new SolidColorBrush(Color.Parse("#9E9E9E"));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}