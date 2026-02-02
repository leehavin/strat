using System.Security.Cryptography;
using System.Text;

namespace Strat.Shared.Utils.Encryption;

/// <summary>
/// AES 加密工具
/// </summary>
public static class AESHelper
{
    /// <summary>
    /// AES 加密
    /// </summary>
    /// <param name="text">明文</param>
    /// <param name="key">密钥（16/24/32位）</param>
    /// <returns>Base64加密串</returns>
    public static string Encrypt(string text, string key)
    {
        using var aes = Aes.Create();
        aes.Key = GetKey(key);
        aes.IV = new byte[16];
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        var plainBytes = Encoding.UTF8.GetBytes(text);
        var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        return Convert.ToBase64String(encryptedBytes);
    }

    /// <summary>
    /// AES 解密
    /// </summary>
    /// <param name="cipherText">密文（Base64）</param>
    /// <param name="key">密钥（16/24/32位）</param>
    /// <returns>明文</returns>
    public static string Decrypt(string cipherText, string key)
    {
        using var aes = Aes.Create();
        aes.Key = GetKey(key);
        aes.IV = new byte[16];
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        var cipherBytes = Convert.FromBase64String(cipherText);
        var decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

        return Encoding.UTF8.GetString(decryptedBytes);
    }

    private static byte[] GetKey(string key)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var result = new byte[32];
        Array.Copy(keyBytes, result, Math.Min(keyBytes.Length, 32));
        return result;
    }
}

