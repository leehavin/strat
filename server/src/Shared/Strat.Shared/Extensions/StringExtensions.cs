namespace Strat.Shared.Extensions;

/// <summary>
/// 字符串扩展方法
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// 判断字符串是否为空或空白
    /// </summary>
    public static bool IsNullOrWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// 判断字符串是否不为空
    /// </summary>
    public static bool IsNotNullOrWhiteSpace(this string? value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// 移除字符串末尾的指定后缀
    /// </summary>
    public static string RemovePostFix(this string value, params string[] postFixes)
    {
        if (string.IsNullOrEmpty(value)) return value;

        foreach (var postFix in postFixes)
        {
            if (value.EndsWith(postFix, StringComparison.OrdinalIgnoreCase))
            {
                return value[..^postFix.Length];
            }
        }

        return value;
    }

    /// <summary>
    /// 移除字符串开头的指定前缀
    /// </summary>
    public static string RemovePreFix(this string value, params string[] preFixes)
    {
        if (string.IsNullOrEmpty(value)) return value;

        foreach (var preFix in preFixes)
        {
            if (value.StartsWith(preFix, StringComparison.OrdinalIgnoreCase))
            {
                return value[preFix.Length..];
            }
        }

        return value;
    }

    /// <summary>
    /// 截取字符串
    /// </summary>
    public static string Truncate(this string value, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            return value;

        return value[..(maxLength - suffix.Length)] + suffix;
    }
}

