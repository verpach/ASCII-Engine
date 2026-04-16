using AsciiWallpaperEngine.Infrastructure.WinApi;
using System.Windows.Forms;

namespace AsciiWallpaperEngine.Infrastructure.Performance;

public sealed class FullscreenDetector
{
    public bool IsForegroundFullscreen()
    {
        var hwnd = NativeMethods.GetForegroundWindow();
        if (hwnd == IntPtr.Zero)
        {
            return false;
        }

        if (!NativeMethods.GetWindowRect(hwnd, out var rect))
        {
            return false;
        }

        foreach (var screen in Screen.AllScreens)
        {
            var b = screen.Bounds;
            if (rect.Left <= b.Left && rect.Top <= b.Top && rect.Right >= b.Right && rect.Bottom >= b.Bottom)
            {
                return true;
            }
        }

        return false;
    }
}
