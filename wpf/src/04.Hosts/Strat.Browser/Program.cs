using System.Runtime.Versioning;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using Avalonia.Media;
using Strat.UI.Base;

[assembly: SupportedOSPlatform("browser")]

internal partial class Program
{
    private static Task Main(string[] args) => BuildAvaloniaApp()
               .StartBrowserAppAsync("out");

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .With(new FontManagerOptions
            {
                DefaultFamilyName = "avares://Strat.Themes/Assets/Fonts/HarmonyOS%20Sans/HarmonyOS_Sans_SC/HarmonyOS_Sans_SC_Regular.ttf#HarmonyOS Sans SC"
            })
            .LogToTrace();
}
