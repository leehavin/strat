namespace Strat.Shared.Exceptions;

/// <summary>
/// 错误码接口
/// </summary>
public interface IErrorCode
{
    int Code { get; }
    string Message { get; }
}

/// <summary>
/// 企业级业务异常基类
/// </summary>
public class StratException : Exception
{
    public int Code { get; }

    public StratException(string message, int code = 500) : base(message)
    {
        Code = code;
    }

    public StratException(IErrorCode errorCode) : base(errorCode.Message)
    {
        Code = errorCode.Code;
    }

    public StratException(IErrorCode errorCode, string customMessage) : base(customMessage)
    {
        Code = errorCode.Code;
    }
}
