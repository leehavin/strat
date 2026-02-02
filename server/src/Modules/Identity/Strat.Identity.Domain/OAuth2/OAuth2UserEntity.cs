namespace Strat.Identity.Domain.OAuth2;

/// <summary>
/// OAuth2 用户实体
/// </summary>
[SugarTable("OAUTH2_USER")]
public class OAuth2UserEntity
{
    /// <summary>
    /// 持久化ID
    /// </summary>
    [SugarColumn(ColumnName = "PERSISTENCE_ID", IsPrimaryKey = true)]
    public long PersistenceId { get; set; }

    /// <summary>
    /// 外部ID
    /// </summary>
    [SugarColumn(ColumnName = "ID")]
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// 用户名称
    /// </summary>
    [SugarColumn(ColumnName = "NAME")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// OAuth2 类型（gitee/github）
    /// </summary>
    [SugarColumn(ColumnName = "TYPE")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 关联的用户ID
    /// </summary>
    [SugarColumn(ColumnName = "USER_ID")]
    public long? UserId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(ColumnName = "CREATE_TIME")]
    public DateTime CreateTime { get; set; }
}

