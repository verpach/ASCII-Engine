using Serilog;
using System.IO;

namespace AsciiWallpaperEngine.Infrastructure.Logging;

public static class LogBootstrapper
{
    public static ILogger Configure()
    {
        var logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AsciiWallpaperEngine", "logs", "log-.txt");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
            .CreateLogger();

        return Log.Logger;
    }
}
