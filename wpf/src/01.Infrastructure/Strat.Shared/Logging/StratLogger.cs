using Serilog;

namespace Strat.Shared.Logging;

public static class StratLogger
{
    public static void Init()
    {
        string folder;
        try
        {
            // 策略 1: 优先尝试使用应用程序运行目录下的 Logs 文件夹
            // 优点: 方便开发调试和运维查找，符合绿色软件习惯
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var localLogs = Path.Combine(basePath, "Logs");
            if (!Directory.Exists(localLogs)) Directory.CreateDirectory(localLogs);

            // 权限检测: 尝试创建一个临时文件以确认是否有写入权限
            // 如果安装在 Program Files 等受限目录，此处会抛出异常
            var testFile = Path.Combine(localLogs, $".write_test_{Guid.NewGuid()}");
            using (File.Create(testFile, 1, FileOptions.DeleteOnClose)) { }
            
            folder = localLogs;
        }
        catch
        {
            // 策略 2: 如果无写入权限，回退到用户 AppData 目录
            // 优点: 符合 Windows 标准规范，确保在受限环境下也能正常记录日志
            folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Strat", "Logs");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        }

        var logPath = Path.Combine(folder, "log-.txt");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Log.Information($"Log initialized. Path: {folder}");
    }

    public static void Information(string message) => Log.Information(message);
    public static void Debug(string message) => Log.Debug(message);
    public static void Warning(string message) => Log.Warning(message);
    public static void Error(string message) => Log.Error(message);
    public static void Error(Exception ex, string message) => Log.Error(ex, message);
}

