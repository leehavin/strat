using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System.Globalization;

namespace Strat.Shared.Converters
{
    /// <summary>
    /// 将资源 Key 字符串转换为 Geometry 的转换器
    /// 企业级标准：支持从应用程序资源中动态查找图标
    /// </summary>
    public class StringToGeometryConverter : IValueConverter
    {
        public static readonly StringToGeometryConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string resourceKey)
            {
                // 尝试从资源中查找
                if (Application.Current != null && Application.Current.TryGetResource(resourceKey, Application.Current.ActualThemeVariant, out var resource))
                {
                    if (resource is Geometry geometry) return geometry;
                }
                
                // 如果资源中找不到，尝试将其解析为 SVG Path 数据
                try
                {
                    return StreamGeometry.Parse(resourceKey);
                }
                catch
                {
                    // 解析失败，忽略
                }
            }

            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
