using AsciiWallpaperEngine.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;

namespace AsciiWallpaperEngine.ViewModels;

public sealed partial class ThemesViewModel : ObservableObject
{
    private readonly AppServices _services;

    public ObservableCollection<ThemeProfile> Themes { get; }

    [ObservableProperty]
    private ThemeProfile? _selectedTheme;

    [ObservableProperty]
    private string _previewText = "";

    public ThemesViewModel(AppServices services)
    {
        _services = services;
        Themes = new ObservableCollection<ThemeProfile>(_services.Config.Themes);
        SelectedTheme = Themes.FirstOrDefault();
    }

    [RelayCommand]
    private void ImportTheme()
    {
        var dialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "ASCII Theme (*.txt;*.json)|*.txt;*.json"
        };

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        var ext = Path.GetExtension(dialog.FileName).ToLowerInvariant();
        var profile = new ThemeProfile
        {
            Name = Path.GetFileNameWithoutExtension(dialog.FileName),
            SourcePath = dialog.FileName,
            SourceType = ext == ".json" ? "animated" : "static"
        };

        _services.Config.Themes.Add(profile);
        Themes.Add(profile);
    }

    [RelayCommand]
    private void Preview()
    {
        if (SelectedTheme is null || !File.Exists(SelectedTheme.SourcePath))
        {
            PreviewText = "Нет данных для предпросмотра";
            return;
        }

        PreviewText = string.Join(Environment.NewLine, File.ReadLines(SelectedTheme.SourcePath).Take(20));
    }
}
