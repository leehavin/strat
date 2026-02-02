using Strat.Identity.Domain.Shared;

namespace Strat.Identity.Domain.Function;

/// <summary>
/// 功能接口关联实体
/// </summary>
[SugarTable("FUNCTION_INTERFACE")]
public class FunctionInterfaceEntity : BaseEntity
{
    /// <summary>
    /// 功能ID
    /// </summary>
    [SugarColumn(ColumnName = "FUNCTION_ID")]
    public long FunctionId { get; set; }

    /// <summary>
    /// 接口ID
    /// </summary>
    [SugarColumn(ColumnName = "INTERFACE_ID")]
    public long InterfaceId { get; set; }
}

