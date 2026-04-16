using CommunityToolkit.Mvvm.ComponentModel;

namespace AsciiWallpaperEngine.ViewModels;

public sealed partial class SettingsViewModel : ObservableObject
{
    private readonly AppServices _services;

    [ObservableProperty]
    private bool _autoStart;

    [ObservableProperty]
    private bool _startMinimizedToTray;

    [ObservableProperty]
    private string _hotkeyNextTheme = "Ctrl+Alt+Right";

    public SettingsViewModel(AppServices services)
    {
        _services = services;
        _autoStart = _services.Config.AutoStart;
        _startMinimizedToTray = _services.Config.StartInTray;
    }

    partial void OnAutoStartChanged(bool value)
    {
        _services.Config.AutoStart = value;
        _services.AutoStartService.SetAutoStart(value, Environment.ProcessPath ?? string.Empty);
    }

    partial void OnStartMinimizedToTrayChanged(bool value) => _services.Config.StartInTray = value;
}
