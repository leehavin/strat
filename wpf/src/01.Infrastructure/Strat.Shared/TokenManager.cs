using System;
using System.IO;

namespace Strat.Shared
{
    public static class TokenManager
    {
        private static readonly string TokenPath;
        private static string? _cachedToken;

        static TokenManager()
        {
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Strat");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            TokenPath = Path.Combine(folder, ".token");
        }

        public static string? GetToken()
        {
            if (_cachedToken != null) return _cachedToken;
            
            if (File.Exists(TokenPath))
            {
                _cachedToken = File.ReadAllText(TokenPath);
                return _cachedToken;
            }
            return null;
        }

        public static void SaveToken(string token)
        {
            _cachedToken = token;
            File.WriteAllText(TokenPath, token);
        }

        public static void ClearToken()
        {
            _cachedToken = null;
            if (File.Exists(TokenPath))
            {
                File.Delete(TokenPath);
            }
        }
    }
}

