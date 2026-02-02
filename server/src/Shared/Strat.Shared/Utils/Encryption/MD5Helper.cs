using System.Security.Cryptography;
using System.Text;

namespace Strat.Shared.Utils.Encryption;

/// <summary>
/// MD5 加密工具
/// </summary>
public static class MD5Helper
{
    /// <summary>
    /// MD5 加密
    /// </summary>
    /// <param name="text">明文</param>
    /// <param name="uppercase">是否大写</param>
    /// <param name="is16">是否16位</param>
    /// <returns>MD5值</returns>
    public static string Encrypt(string text, bool uppercase = false, bool is16 = false)
    {
        var bytes = Encoding.UTF8.GetBytes(text);
        return Encrypt(bytes, uppercase, is16);
    }

    /// <summary>
    /// MD5 加密
    /// </summary>
    /// <param name="bytes">字节数组</param>
    /// <param name="uppercase">是否大写</param>
    /// <param name="is16">是否16位</param>
    /// <returns>MD5值</returns>
    public static string Encrypt(byte[] bytes, bool uppercase = false, bool is16 = false)
    {
        var hashBytes = MD5.HashData(bytes);
        var sb = new StringBuilder();

        foreach (var b in hashBytes)
        {
            sb.Append(b.ToString("x2"));
        }

        var hash = sb.ToString();
        if (is16) hash = hash.Substring(8, 16);
        return uppercase ? hash.ToUpper() : hash;
    }

    /// <summary>
    /// 验证 MD5
    /// </summary>
    public static bool Verify(string text, string hash, bool uppercase = false, bool is16 = false)
    {
        var computed = Encrypt(text, uppercase, is16);
        return computed.Equals(hash, StringComparison.OrdinalIgnoreCase);
    }
}

