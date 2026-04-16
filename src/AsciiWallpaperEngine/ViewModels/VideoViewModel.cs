using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AsciiWallpaperEngine.ViewModels;

public sealed partial class VideoViewModel : ObservableObject
{
    private readonly AppServices _services;

    [ObservableProperty]
    private string _videoPath;

    [ObservableProperty]
    private string _videoQuality = "Среднее";

    [ObservableProperty]
    private int _videoFps;

    public VideoViewModel(AppServices services)
    {
        _services = services;
        _videoPath = _services.Config.VideoPath ?? string.Empty;
        _videoFps = _services.Config.TargetFps;
    }

    [RelayCommand]
    private void BrowseVideo()
    {
        var dialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "Video (*.mp4;*.avi;*.mkv)|*.mp4;*.avi;*.mkv"
        };

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        VideoPath = dialog.FileName;
        _services.Config.VideoPath = dialog.FileName;
    }

    [RelayCommand]
    private void ApplyVideoSettings()
    {
        _services.Config.TargetFps = VideoFps;
    }
}
