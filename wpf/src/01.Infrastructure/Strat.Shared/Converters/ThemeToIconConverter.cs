using Avalonia.Data.Converters;
using Strat.Shared.Assets;
using Strat.Shared.Services;
using System.Globalization;

namespace Strat.Shared.Converters
{
    public class ThemeToIconConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is ThemeMode mode)
            {
                return mode == ThemeMode.Dark ? SemiIcons.Sun : SemiIcons.Moon;
            }
            return SemiIcons.Moon;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
