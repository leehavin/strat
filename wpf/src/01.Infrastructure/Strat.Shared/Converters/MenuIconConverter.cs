using System.Globalization;
using Avalonia.Data.Converters;

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


