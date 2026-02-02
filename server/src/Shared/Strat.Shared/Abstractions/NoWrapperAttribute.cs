namespace Strat.Shared.Abstractions;

/// <summary>
/// 标记控制器或操作不进行结果包装
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class NoWrapperAttribute : Attribute
{
}

