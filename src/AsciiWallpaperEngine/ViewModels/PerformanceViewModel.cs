using CommunityToolkit.Mvvm.ComponentModel;

namespace AsciiWallpaperEngine.ViewModels;

public sealed partial class PerformanceViewModel : ObservableObject
{
    private readonly AppServices _services;

    [ObservableProperty]
    private int _targetFps;

    [ObservableProperty]
    private bool _pauseFullscreen;

    [ObservableProperty]
    private bool _pauseOnBattery;

    public PerformanceViewModel(AppServices services)
    {
        _services = services;
        _targetFps = _services.Config.TargetFps;
        _pauseFullscreen = _services.Config.PauseOnFullscreen;
        _pauseOnBattery = _services.Config.PauseOnBattery;
    }

    partial void OnTargetFpsChanged(int value) => _services.Config.TargetFps = value;
    partial void OnPauseFullscreenChanged(bool value) => _services.Config.PauseOnFullscreen = value;
    partial void OnPauseOnBatteryChanged(bool value) => _services.Config.PauseOnBattery = value;
}
