using System.Globalization;
using Microsoft.Maui.Controls; // Assuming you're using MAUI

namespace mauiRPG.Converters;

public class IntToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int intValue && parameter is string stringParameter)
        {
            if (int.TryParse(stringParameter, out int compareValue))
            {
                return intValue == compareValue;
            }
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}