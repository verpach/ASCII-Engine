using Microsoft.Win32;

namespace AsciiWallpaperEngine.Infrastructure.Startup;

public sealed class AutoStartService
{
    private const string RunPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string AppName = "AsciiWallpaperEngine";

    public void SetAutoStart(bool enabled, string exePath)
    {
        if (string.IsNullOrWhiteSpace(exePath))
        {
            return;
        }

        using var key = Registry.CurrentUser.OpenSubKey(RunPath, writable: true);
        if (key is null)
        {
            return;
        }

        if (enabled)
        {
            key.SetValue(AppName, $"\"{exePath}\" --tray");
            return;
        }

        key.DeleteValue(AppName, throwOnMissingValue: false);
    }
}
