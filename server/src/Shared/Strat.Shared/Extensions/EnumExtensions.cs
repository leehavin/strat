using System.Reflection;

namespace Strat.Shared.Extensions;

/// <summary>
/// 枚举扩展方法
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// 获取枚举的描述
    /// </summary>
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field == null) return value.ToString();

        var attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }

    /// <summary>
    /// 将枚举转换为键值对列表
    /// </summary>
    public static List<KeyValuePair<int, string>> ToList<TEnum>() where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Select(e => new KeyValuePair<int, string>(
                Convert.ToInt32(e),
                e.GetDescription()))
            .ToList();
    }
}

