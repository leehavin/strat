

using Serilog;

namespace Strat.Shared.Logging;

public static class StratLogger
{
    public static void Init()
    {
        var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Strat", "Logs");
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        var logPath = Path.Combine(folder, "log-.txt");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    public static void Information(string message) => Log.Information(message);
    public static void Debug(string message) => Log.Debug(message);
    public static void Warning(string message) => Log.Warning(message);
    public static void Error(string message) => Log.Error(message);
    public static void Error(Exception ex, string message) => Log.Error(ex, message);
}

