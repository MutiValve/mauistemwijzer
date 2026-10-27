using System.Globalization;

namespace mauistemwijzer.Converters;

/// <summary>
/// Converts a hex color string (e.g. "#FF6600") to a MAUI Color for use in XAML bindings.
/// </summary>
public class StringToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string colorStr && !string.IsNullOrWhiteSpace(colorStr))
        {
            try { return Color.FromArgb(colorStr); }
            catch { /* fall through to default */ }
        }
        return Colors.Gray;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
