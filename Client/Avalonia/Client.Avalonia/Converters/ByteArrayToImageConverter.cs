using System;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

namespace Client.Avalonia.Converters;

public class ByteArrayToImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is byte[] imageData && imageData.Length > 0)
        {
            try
            {
                using var ms = new MemoryStream(imageData);
                return new Bitmap(ms);
            }
            catch
            {
                return null;
            }
        }
            
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}