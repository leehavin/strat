using Strat.System.Domain.Shared;

namespace Strat.System.Domain.Config;

/// <summary>
/// 系统配置实体
/// </summary>
[SugarTable("SYSTEM_CONFIG")]
public class SystemConfigEntity : BaseEntity
{
    /// <summary>
    /// 配置名称
    /// </summary>
    [SugarColumn(ColumnName = "NAME")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 配置编码
    /// </summary>
    [SugarColumn(ColumnName = "CONFIG_CODE")]
    public string ConfigCode { get; set; } = string.Empty;

    /// <summary>
    /// 配置值
    /// </summary>
    [SugarColumn(ColumnName = "CONFIG_VALUE")]
    public string ConfigValue { get; set; } = string.Empty;
}

