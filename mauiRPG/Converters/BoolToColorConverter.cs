using System.Globalization;
using Microsoft.Maui.Controls; 

namespace mauiRPG.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            if (value is bool isPlayerAction)
            {
                return isPlayerAction ? Colors.LightBlue : Colors.LightPink;
            }
            return Colors.White; // Default color
        }

        public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotImplementedException();
        }
    }
}