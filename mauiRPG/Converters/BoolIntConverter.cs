using System.Diagnostics;
using System.Globalization;

namespace mauiRPG.Converters
{
    public class BoolIntConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not int intValue || parameter is not string stringParameter) return false;
            if (int.TryParse(stringParameter, out int compareValue))
            {
                return intValue == compareValue;
            }
            return false;
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
