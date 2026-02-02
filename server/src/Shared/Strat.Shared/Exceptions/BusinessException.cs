using System.ComponentModel;
using System.Reflection;
using Volo.Abp;

namespace Strat.Shared.Exceptions;

/// <summary>
/// 业务异常辅助类
/// </summary>
public static class BusinessException
{
    /// <summary>
    /// 抛出业务异常信息
    /// </summary>
    /// <param name="errorMessage">异常消息</param>
    /// <returns>异常实例</returns>
    public static StratException Throw(string errorMessage)
    {
        return new StratException(errorMessage, 400);
    }

    /// <summary>
    /// 抛出业务异常信息（带错误码）
    /// </summary>
    /// <param name="errorMessage">异常消息</param>
    /// <param name="errorCode">错误码</param>
    /// <returns>异常实例</returns>
    public static StratException Throw(string errorMessage, int errorCode)
    {
        return new StratException(errorMessage, errorCode);
    }

    /// <summary>
    /// 抛出业务异常信息（基于错误码接口）
    /// </summary>
    public static StratException Throw(IErrorCode errorCode)
    {
        return new StratException(errorCode);
    }

    /// <summary>
    /// 根据枚举抛出业务异常
    /// </summary>
    /// <param name="tipsEnum">错误提示枚举</param>
    /// <returns>异常实例</returns>
    public static StratException Throw(Enum tipsEnum)
    {
        var message = GetEnumDescription(tipsEnum);
        return new StratException(message, 400);
    }

    private static string GetEnumDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }
}

