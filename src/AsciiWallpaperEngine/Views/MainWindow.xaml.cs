using AsciiWallpaperEngine.ViewModels;
using NHotkey;
using NHotkey.Wpf;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace AsciiWallpaperEngine.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        Closing += OnClosing;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        HotkeyManager.Current.AddOrReplace(
            "NextTheme",
            Key.Right,
            ModifierKeys.Control | ModifierKeys.Alt,
            (_, _) => App.Services.SceneManager.NextTheme());
    }

    private void OnClosing(object? sender, CancelEventArgs e)
    {
        if (DataContext is MainViewModel vm && vm.Settings.StartMinimizedToTray)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
