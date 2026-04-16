using AsciiWallpaperEngine.Core.Models;
using AsciiWallpaperEngine.Infrastructure.Performance;
using System.Windows.Forms;

namespace AsciiWallpaperEngine.Engine;

public sealed class PerformanceModeService
{
    private readonly EngineConfig _config;
    private readonly FullscreenDetector _fullscreenDetector = new();

    public PerformanceModeService(EngineConfig config)
    {
        _config = config;
    }

    public bool ShouldPause()
    {
        if (_config.PauseOnBattery && SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Offline)
        {
            return true;
        }

        return _config.PauseOnFullscreen && _fullscreenDetector.IsForegroundFullscreen();
    }
}
