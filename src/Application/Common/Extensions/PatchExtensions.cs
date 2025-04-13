using System;

namespace Application.Common.Extensions;

public static class PatchExtensions
{
    public static void SetIfNotNullOrWhiteSpace(this string? value, Action<string> setter)
    {
        if (!string.IsNullOrWhiteSpace(value))
            setter(value);
    }

    public static void SetIfNotNull<T>(this T? value, Action<T> setter) where T : struct
    {
        if (value.HasValue)
            setter(value.Value);
    }

    public static void SetIfNotNull<T>(this T? value, Action<T> setter) where T : class
    {
        if (value is not null)
            setter(value);
    }
}
