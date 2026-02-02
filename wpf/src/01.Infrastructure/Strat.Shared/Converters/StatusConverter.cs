using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Strat.Shared.Converters
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int status)
            {
                return status == 1 ? "正常" : "停用";
            }
            return "未知";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int status)
            {
                return status == 1;
            }
            return false;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToInverseBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int status)
            {
                return status != 1;
            }
            return false;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToBackgroundConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int status)
            {
                return status == 1 
                    ? new SolidColorBrush(Color.FromRgb(232, 255, 234))  // 绿色背景
                    : new SolidColorBrush(Color.FromRgb(255, 242, 232)); // 橙色背景
            }
            return new SolidColorBrush(Color.FromRgb(240, 240, 240));
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToForegroundConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int status)
            {
                return status == 1 
                    ? new SolidColorBrush(Color.FromRgb(56, 158, 13))   // 绿色文字
                    : new SolidColorBrush(Color.FromRgb(212, 107, 8)); // 橙色文字
            }
            return new SolidColorBrush(Color.FromRgb(128, 128, 128));
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

