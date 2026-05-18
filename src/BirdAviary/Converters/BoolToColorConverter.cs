using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BirdAviary.Converters;

public class BoolToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var isSuccess = value is true;
        return new SolidColorBrush(isSuccess
            ? Color.FromRgb(46, 204, 113)
            : Color.FromRgb(231, 76, 60));
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
