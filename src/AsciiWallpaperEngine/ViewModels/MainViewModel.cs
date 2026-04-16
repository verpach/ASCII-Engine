using AsciiWallpaperEngine.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AsciiWallpaperEngine.ViewModels;

public sealed partial class MainViewModel : ObservableObject
{
    private readonly AppServices _services;

    [ObservableProperty]
    private bool _isWallpaperEnabled;

    [ObservableProperty]
    private ThemeProfile? _selectedProfile;

    public ThemesViewModel Themes { get; }
    public VideoViewModel Video { get; }
    public PerformanceViewModel Performance { get; }
    public SettingsViewModel Settings { get; }

    public MainViewModel(AppServices services)
    {
        _services = services;
        Themes = new ThemesViewModel(services);
        Video = new VideoViewModel(services);
        Performance = new PerformanceViewModel(services);
        Settings = new SettingsViewModel(services);

        IsWallpaperEnabled = _services.Config.Enabled;
        SelectedProfile = _services.Config.Themes.FirstOrDefault(t => t.Name == _services.Config.SelectedThemeName);
    }

    [RelayCommand]
    private void ToggleWallpaper()
    {
        _services.Config.Enabled = IsWallpaperEnabled;
    }

    [RelayCommand]
    private void ApplyProfile()
    {
        if (SelectedProfile is null)
        {
            return;
        }

        _services.Config.SelectedThemeName = SelectedProfile.Name;
    }
}
