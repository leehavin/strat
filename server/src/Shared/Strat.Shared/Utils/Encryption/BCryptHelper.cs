using BCrypt.Net;

namespace Strat.Shared.Utils.Encryption;

/// <summary>
/// BCrypt 密码加密工具
/// </summary>
public static class BCryptHelper
{
    /// <summary>
    /// 加密密码
    /// </summary>
    /// <param name="password">明文密码</param>
    /// <returns>哈希值</returns>
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// 验证密码
    /// </summary>
    /// <param name="password">明文密码</param>
    /// <param name="hash">哈希值</param>
    /// <returns>是否匹配</returns>
    public static bool VerifyPassword(string password, string hash)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
            return false;

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        catch
        {
            return false;
        }
    }
}

