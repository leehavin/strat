namespace Strat.System.Application.Contracts.Config.Dtos;

public class SystemConfigResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ConfigCode { get; set; } = string.Empty;
    public string ConfigValue { get; set; } = string.Empty;
    public string? Remark { get; set; }
}

