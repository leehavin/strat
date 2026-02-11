using Avalonia.Data.Converters;
using System.Globalization;

namespace Strat.Shared.Converters
{
    public class MenuIconConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isExpanded)
            {
                return isExpanded ? "MenuFold" : "MenuUnfold";
            }
            return "MenuUnfold";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


