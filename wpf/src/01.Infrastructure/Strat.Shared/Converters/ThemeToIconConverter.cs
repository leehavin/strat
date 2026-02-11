using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
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
                string resourceKey = mode == ThemeMode.Dark ? SemiIcons.Sun : SemiIcons.Moon;
                
                // 尝试从资源中查找 Geometry
                if (Application.Current != null && Application.Current.TryGetResource(resourceKey, Application.Current.ActualThemeVariant, out var resource))
                {
                    if (resource is Geometry geometry) return geometry;
                }
                
                // 如果找不到资源，返回字符串（由 StringToGeometryConverter 处理，如果作为参数传递）
                return resourceKey;
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
