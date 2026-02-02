using System.Security.Claims;

namespace Strat.Shared.Abstractions;

/// <summary>
/// Token 服务接口
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// 生成 Token
    /// </summary>
    /// <param name="claims">声明列表</param>
    /// <returns>Token 字符串</returns>
    string GenerateToken(IEnumerable<Claim> claims);

    /// <summary>
    /// 获取刷新时间（分钟）
    /// </summary>
    /// <returns>刷新时间</returns>
    double GetRefreshMinutes();
}

