using System.ComponentModel;
using System.Reflection;
using Volo.Abp;

namespace Strat.Shared.Exceptions;

/// <summary>
/// 验证异常辅助类
/// </summary>
public  class ValidationException
{
    /// <summary>
    /// 抛出验证异常信息
    /// </summary>
    /// <param name="errorMessage">异常消息</param>
    /// <returns>异常实例</returns>
    public static UserFriendlyException Throw(string errorMessage)
    {
        return new UserFriendlyException(errorMessage, "VALIDATION_ERROR");
    }

    /// <summary>
    /// 根据枚举抛出验证异常
    /// </summary>
    /// <param name="tipsEnum">错误提示枚举</param>
    /// <returns>异常实例</returns>
    public static UserFriendlyException Throw(Enum tipsEnum)
    {
        var message = GetEnumDescription(tipsEnum);
        return new UserFriendlyException(message, "VALIDATION_ERROR");
    }

    private static string GetEnumDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }
}

