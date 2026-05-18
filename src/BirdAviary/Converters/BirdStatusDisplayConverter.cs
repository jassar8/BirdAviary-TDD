using System.Globalization;
using System.Windows.Data;
using BirdAviary.Core.Enums;
using BirdAviary.Helpers;

namespace BirdAviary.Converters;

public class BirdStatusDisplayConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is BirdStatus status)
            return EnumDisplayHelper.GetBirdStatusDisplay(status);
        return value?.ToString() ?? string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
