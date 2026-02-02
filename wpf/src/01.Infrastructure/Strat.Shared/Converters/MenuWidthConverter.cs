using System.Globalization;
using Avalonia.Data.Converters;

namespace Strat.Shared.Converters
{
    public class MenuWidthConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isExpanded)
            {
                return isExpanded ? 200 : 64;
            }
            return 64;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


