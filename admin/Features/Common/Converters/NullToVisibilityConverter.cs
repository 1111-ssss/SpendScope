using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace admin.Features.Common.Converters;

public sealed class NullToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Enum && (value.ToString() == "None" || value.ToString() == "Empty"))
            return Visibility.Collapsed;

        return value is null ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
