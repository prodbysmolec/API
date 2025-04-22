using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Client.Avalonia.Converters;

public class EnumDescriptionConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null) return string.Empty;

        var type = value.GetType();
        if (!type.IsEnum) return value.ToString();

        var name = Enum.GetName(type, value);
        if (name == null) return value.ToString();

        var field = type.GetField(name);
        var attr = field?.GetCustomAttribute<DescriptionAttribute>();

        return attr?.Description ?? name;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        foreach (var field in targetType.GetFields())
        {
            var attr = field.GetCustomAttribute<DescriptionAttribute>();
            if ((attr != null && attr.Description == value.ToString()) || field.Name == value.ToString())
            {
                return Enum.Parse(targetType, field.Name);
            }
        }

        return BindingOperations.DoNothing;
    }
}