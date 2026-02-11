using System.Globalization;

namespace Strat.Shared.Services
{
    public class LanguageInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty; // e.g. "zh-CN", "en-US"
        public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;
    }

    public interface ILocalizationService
    {
        LanguageInfo CurrentLanguage { get; }
        IEnumerable<LanguageInfo> SupportedLanguages { get; }
        void SetLanguage(string key);
    }
}
