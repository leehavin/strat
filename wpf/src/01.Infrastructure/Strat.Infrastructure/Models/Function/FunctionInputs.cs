namespace Strat.Infrastructure.Models.Function;

public class AddFunctionInput
{
    public long? ParentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int Type { get; set; } // 0:Directory, 1:Menu, 2:Button
    public int Sort { get; set; }
    public string? Icon { get; set; }
    public string? Path { get; set; }
    public string? Component { get; set; }
    public bool Visible { get; set; } = true;
    public int Status { get; set; } = 1; // 1:Enabled, 0:Disabled
    public string? Remark { get; set; }
}

public class UpdateFunctionInput : AddFunctionInput
{
    public long Id { get; set; }
}
