using Avalonia.Data.Converters;
using Avalonia.Media;
using System.Globalization;

namespace Strat.Shared.Converters
{
    public class BoolToWeightConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isTrue && isTrue) return FontWeight.Bold;
            return FontWeight.Normal;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToGreyConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool isLast = value is bool b && b;
            string param = parameter as string ?? "";

            if (param == "Primary")
            {
                return isLast ? new FuncValueConverter<bool, IBrush>(_ => null!) : null; // Logic depends on dynamic resource names usually
            }
            
            // For simplicity, we return specific color keys used in enterprise layout
            return isLast ? "SemiBlue6" : "SemiGrey6";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
