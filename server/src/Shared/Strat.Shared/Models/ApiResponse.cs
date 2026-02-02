using System.Diagnostics;

namespace Strat.Shared.Models;

/// <summary>
/// 统一API返回结果
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// 业务状态码
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 业务数据
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// 提示消息
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 链路追踪ID
    /// </summary>
    public string? TraceId { get; set; }

    /// <summary>
    /// 成功结果
    /// </summary>
    public static ApiResponse<T> Success(T? data = default, string message = "success")
    {
        return new ApiResponse<T>
        {
            Code = 200,
            Data = data,
            Message = message,
            TraceId = Activity.Current?.TraceId.ToString()
        };
    }

    /// <summary>
    /// 失败结果
    /// </summary>
    public static ApiResponse<T> Error(string message = "error", int code = 500)
    {
        return new ApiResponse<T>
        {
            Code = code,
            Data = default,
            Message = message,
            TraceId = Activity.Current?.TraceId.ToString()
        };
    }
}

/// <summary>
/// 统一API返回结果（无泛型）
/// </summary>
public class ApiResponse : ApiResponse<object>
{
    /// <summary>
    /// 成功结果
    /// </summary>
    public static new ApiResponse Success(object? data = null, string message = "success")
    {
        return new ApiResponse
        {
            Code = 200,
            Data = data,
            Message = message,
            TraceId = Activity.Current?.TraceId.ToString()
        };
    }

    /// <summary>
    /// 失败结果
    /// </summary>
    public static new ApiResponse Error(string message = "error", int code = 500)
    {
        return new ApiResponse
        {
            Code = code,
            Data = null,
            Message = message,
            TraceId = Activity.Current?.TraceId.ToString()
        };
    }
}

