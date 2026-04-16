using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace AsciiWallpaperEngine.Views;

public partial class WallpaperWindow : Window
{
    public WallpaperWindow()
    {
        InitializeComponent();
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        var hwnd = new WindowInteropHelper(this).Handle;
        App.Services.WallpaperHost.AttachToDesktop(hwnd, App.Services.Config.Clickable);
    }

    public void UpdateBitmap(WriteableBitmap bitmap)
    {
        Dispatcher.InvokeAsync(() => WallpaperImage.Source = bitmap);
    }
}
