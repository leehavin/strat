namespace Strat.Shared.Enums;

/// <summary>
/// 存储类型枚举
/// </summary>
public enum StorageTypeEnum
{
    /// <summary>
    /// 本地存储
    /// </summary>
    [Description("本地存储")]
    Local = 0,

    /// <summary>
    /// 阿里云OSS
    /// </summary>
    [Description("阿里云OSS")]
    AliyunOss = 1,

    /// <summary>
    /// 腾讯云COS
    /// </summary>
    [Description("腾讯云COS")]
    TencentCos = 2
}

