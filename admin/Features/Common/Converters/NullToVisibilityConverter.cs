using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace admin.Features.Common.Converters;

public sealed class NullToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch {
            Enum e => (value.ToString() == "None" || value.ToString() == "Empty")
                    ? Visibility.Collapsed
                    : Visibility.Visible,
            string s => string.IsNullOrEmpty(s)
                    ? Visibility.Collapsed
                    : Visibility.Visible,
            null => Visibility.Collapsed,
            _ => Visibility.Visible,
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
