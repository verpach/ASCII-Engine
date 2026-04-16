using AsciiWallpaperEngine.Core.Interfaces;

namespace AsciiWallpaperEngine.Infrastructure.WinApi;

public sealed class WallpaperHost : IWallpaperHost
{
    public IntPtr FindWorkerW()
    {
        var progman = NativeMethods.FindWindow("Progman", null);
        if (progman == IntPtr.Zero)
        {
            throw new InvalidOperationException("Progman not found");
        }

        _ = NativeMethods.SendMessageTimeout(
            progman,
            NativeMethods.WM_SPAWN_WORKER,
            IntPtr.Zero,
            IntPtr.Zero,
            NativeMethods.SMTO_NORMAL,
            1000,
            out _);

        IntPtr workerW = IntPtr.Zero;

        NativeMethods.EnumWindows((topHandle, _) =>
        {
            var shellView = NativeMethods.FindWindowEx(topHandle, IntPtr.Zero, "SHELLDLL_DefView", null);
            if (shellView != IntPtr.Zero)
            {
                workerW = NativeMethods.FindWindowEx(IntPtr.Zero, topHandle, "WorkerW", null);
            }

            return true;
        }, IntPtr.Zero);

        if (workerW == IntPtr.Zero)
        {
            throw new InvalidOperationException("WorkerW not found");
        }

        return workerW;
    }

    public void AttachToDesktop(IntPtr wallpaperWindow, bool clickable)
    {
        var workerW = FindWorkerW();
        NativeMethods.SetParent(wallpaperWindow, workerW);

        var style = NativeMethods.GetWindowLong(wallpaperWindow, NativeMethods.GWL_STYLE);
        style |= NativeMethods.WS_CHILD | NativeMethods.WS_VISIBLE;
        NativeMethods.SetWindowLong(wallpaperWindow, NativeMethods.GWL_STYLE, style);

        var exStyle = NativeMethods.GetWindowLong(wallpaperWindow, NativeMethods.GWL_EXSTYLE);
        exStyle |= NativeMethods.WS_EX_TOOLWINDOW | NativeMethods.WS_EX_NOACTIVATE;
        if (!clickable)
        {
            exStyle |= NativeMethods.WS_EX_TRANSPARENT;
        }

        NativeMethods.SetWindowLong(wallpaperWindow, NativeMethods.GWL_EXSTYLE, exStyle);
    }
}
