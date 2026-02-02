namespace Strat.System.Application.Contracts.Config.Dtos;

public class AddSystemConfigRequest
{
    [Required(ErrorMessage = "配置名称不能为空")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "配置编码不能为空")]
    [MaxLength(100)]
    public string ConfigCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "配置值不能为空")]
    public string ConfigValue { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Remark { get; set; }
}

