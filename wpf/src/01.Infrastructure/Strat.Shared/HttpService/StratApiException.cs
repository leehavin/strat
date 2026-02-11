namespace Strat.Shared.HttpService
{
    /// <summary>
    /// 企业级 API 异常基类
    /// </summary>
    public class StratApiException : Exception
    {
        public int StatusCode { get; }
        public int ApiCode { get; }
        public string? TraceId { get; }

        public StratApiException(string message, int statusCode = 500, int apiCode = 500, string? traceId = null) 
            : base(message)
        {
            StatusCode = statusCode;
            ApiCode = apiCode;
            TraceId = traceId;
        }
    }
}

