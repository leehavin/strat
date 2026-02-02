using System.Text.Json.Serialization;

namespace Strat.Shared.Models
{
    /// <summary>
    /// 企业级统一 API 响应包装
    /// 基础设施层共享模型，所有项目通用
    /// </summary>
    /// <typeparam name="T">响应数据类型</typeparam>
    public class ApiResponse<T>
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        
        [JsonPropertyName("data")]
        public T? Data { get; set; }
        
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("traceId")]
        public string? TraceId { get; set; }

        public bool Success => Code == 200;
    }

    /// <summary>
    /// 企业级统一 API 响应包装（无数据）
    /// </summary>
    public class ApiResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("traceId")]
        public string? TraceId { get; set; }

        public bool Success => Code == 200;
    }
}
