using Avalonia;
using Avalonia.Styling;
using Semi.Avalonia;
using Strat.Shared.Logging;

namespace Strat.Shared.Services
{
    public class ThemeService : IThemeService
    {
        public ThemeMode CurrentMode { get; private set; } = ThemeMode.Light;

        public void SetTheme(ThemeMode mode)
        {
            if (Application.Current == null) return;

            var theme = Application.Current.Styles.OfType<SemiTheme>().FirstOrDefault();
            if (theme == null)
            {
                StratLogger.Error("[Theme] 未能在 Application.Styles 中找到 SemiTheme");
                return;
            }

            CurrentMode = mode;
            StratLogger.Information($"[Theme] 设置主题 Mode: {mode}");
            
            switch (mode)
            {
                case ThemeMode.Light:
                    Application.Current.RequestedThemeVariant = ThemeVariant.Light;
                    break;
                case ThemeMode.Dark:
                    Application.Current.RequestedThemeVariant = ThemeVariant.Dark;
                    break;
                case ThemeMode.System:
                    Application.Current.RequestedThemeVariant = ThemeVariant.Default;
                    break;
            }

            StratLogger.Information($"[Theme] Switched to {mode}");
        }

        public void ToggleTheme()
        {
            SetTheme(CurrentMode == ThemeMode.Light ? ThemeMode.Dark : ThemeMode.Light);
        }
    }
}
