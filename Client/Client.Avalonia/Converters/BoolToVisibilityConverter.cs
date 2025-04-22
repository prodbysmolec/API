using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Client.Avalonia.Converters;

public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            bool invert = parameter as string == "invert";
            boolValue = invert ? !boolValue : boolValue;
                
            return boolValue;
        }
            
        // Wenn value nicht null ist, wird es als "sichtbar" betrachtet
        if (value != null)
        {
            bool invert = parameter as string == "invert";
            return invert ? false : true;
        }
            
        // Standardmäßig "nicht sichtbar"
        bool defaultInvert = parameter as string == "invert";
        return defaultInvert ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}