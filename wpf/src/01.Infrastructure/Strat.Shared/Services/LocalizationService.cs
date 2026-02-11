using Avalonia;
using Avalonia.Markup.Xaml.Styling;
using Strat.Shared.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Strat.Shared.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly List<LanguageInfo> _languages = new()
        {
            new LanguageInfo { Name = "简体中文", Key = "zh-CN", Culture = new CultureInfo("zh-CN") },
            new LanguageInfo { Name = "English", Key = "en-US", Culture = new CultureInfo("en-US") }
        };

        public LanguageInfo CurrentLanguage { get; private set; }
        public IEnumerable<LanguageInfo> SupportedLanguages => _languages;

        public LocalizationService()
        {
            CurrentLanguage = _languages[0]; // Default zh-CN
        }

        public void SetLanguage(string key)
        {
            var lang = _languages.FirstOrDefault(x => x.Key == key);
            if (lang == null || Application.Current == null) return;

            try
            {
                var uri = new Uri($"avares://Strat.Themes/I18n/{key}.axaml");
                var newDict = new ResourceInclude(new Uri("avares://Strat.Themes/")) { Source = uri };

                // Find existing I18n dictionary
                var mergedDicts = Application.Current.Resources.MergedDictionaries;
                var oldDict = mergedDicts.OfType<ResourceInclude>()
                    .FirstOrDefault(x => x.Source?.ToString().Contains("/I18n/") == true);

                if (oldDict != null)
                {
                    mergedDicts.Remove(oldDict);
                }
                mergedDicts.Add(newDict);
                
                CurrentLanguage = lang;
                CultureInfo.CurrentUICulture = lang.Culture;
                
                StratLogger.Information($"[I18n] Switched to {key}");
            }
            catch (Exception ex)
            {
                StratLogger.Error($"[I18n] Failed to switch to {key}: {ex.Message}");
            }
        }
    }
}
