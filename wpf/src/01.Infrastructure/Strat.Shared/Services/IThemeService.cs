namespace Strat.Shared.Services
{
    public enum ThemeMode
    {
        Light,
        Dark,
        System
    }

    public interface IThemeService
    {
        ThemeMode CurrentMode { get; }
        void SetTheme(ThemeMode mode);
        void ToggleTheme();
    }
}
