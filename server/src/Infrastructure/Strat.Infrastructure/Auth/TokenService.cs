using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Strat.Shared.Abstractions;
using Volo.Abp.Timing;

namespace Strat.Infrastructure.Auth;

/// <summary>
/// Token 服务实现
/// </summary>
public class TokenService(IConfiguration configuration, IClock clock) : ITokenService, ISingletonDependency
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IClock _clock = clock;

    /// <summary>
    /// 生成 Token
    /// </summary>
    public string GenerateToken(IEnumerable<Claim> claims)
    {
        var secretKey = _configuration["JwtOptions:SecretKey"] ?? "DefaultSecretKey1234567890123456";
        var expiredMinutes = _configuration.GetValue("JwtOptions:ExpiredMinutes", 60);

        var key = Encoding.UTF8.GetBytes(secretKey);
        var signingKey = new SymmetricSecurityKey(key);
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            notBefore: _clock.Now,
            expires: _clock.Now.AddMinutes(expiredMinutes),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// 获取刷新时间
    /// </summary>
    public double GetRefreshMinutes()
    {
        return _configuration.GetValue("JwtOptions:RefreshMinutes", 30);
    }
}

