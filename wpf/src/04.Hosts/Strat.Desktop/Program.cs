using Avalonia;
using Avalonia.Media;
using System;

namespace Strat.Desktop;

sealed class Program
{
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<Strat.UI.Base.App>()
            .UsePlatformDetect()
            .With(new FontManagerOptions
            {
                DefaultFamilyName = "avares://Strat.Themes/Assets/Fonts/HarmonyOS%20Sans/HarmonyOS_Sans_SC/HarmonyOS_Sans_SC_Regular.ttf#HarmonyOS Sans SC"
            })
            .LogToTrace();
}
