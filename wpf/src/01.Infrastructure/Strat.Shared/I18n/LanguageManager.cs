using Avalonia;
using Avalonia.Markup.Xaml.Styling;
using System.Globalization;

namespace Strat.Shared.I18n
{
    public class LanguageItem
    {
        public string Name { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
    }

    public class LanguageManager : BindableBase
    {
        private const string I18N_RESOURCE_PREFIX = "avares://Strat.Themes/I18n/";
        
        public static LanguageManager Instance { get; } = new LanguageManager();

        public List<LanguageItem> SupportedLanguages { get; } = new()
        {
            new() { Name = "简体中文", Key = "zh-CN" },
            new() { Name = "繁體中文", Key = "zh-TW" },
            new() { Name = "English", Key = "en-US" },
            new() { Name = "Deutsch", Key = "de-DE" },
            new() { Name = "日本語", Key = "ja-JP" },
            new() { Name = "한국어", Key = "ko-KR" },
            new() { Name = "Français", Key = "fr-FR" },
            new() { Name = "Español", Key = "es-ES" }
        };

        private string _currentCulture = "zh-CN";
        public string CurrentCulture
        {
            get => _currentCulture;
            private set => SetProperty(ref _currentCulture, value);
        }

        private LanguageManager()
        {
        }

        /// <summary>
        /// 切换语言
        /// </summary>
        /// <param name="cultureName">语言代码，如 en-US, zh-CN</param>
        public void SwitchLanguage(string cultureName)
        {
            if (string.IsNullOrEmpty(cultureName)) return;

            var app = Application.Current;
            if (app == null) return;

            // 1. 构造新的资源路径
            var source = new Uri($"{I18N_RESOURCE_PREFIX}{cultureName}.axaml");
            
            // 2. 查找并替换旧的语言资源
            var oldDict = app.Resources.MergedDictionaries
                .OfType<ResourceInclude>()
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.StartsWith(I18N_RESOURCE_PREFIX));

            if (oldDict != null)
            {
                app.Resources.MergedDictionaries.Remove(oldDict);
            }

            // 3. 添加新资源
            app.Resources.MergedDictionaries.Add(new ResourceInclude(source) { Source = source });

            // 4. 更新当前状态
            CurrentCulture = cultureName;
            CultureInfo.CurrentUICulture = new CultureInfo(cultureName);
            
        }

        /// <summary>
        /// 获取当前语言资源中的字符串
        /// </summary>
        public string GetString(string key)
        {
            if (Application.Current != null && Application.Current.TryGetResource(key, Application.Current.ActualThemeVariant, out var value) && value is string str)
            {
                return str;
            }
            return key;
        }
    }
}
