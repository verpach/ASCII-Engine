namespace AsciiWallpaperEngine.Core.Interfaces;

public interface IWallpaperHost
{
    IntPtr FindWorkerW();
    void AttachToDesktop(IntPtr wallpaperWindow, bool clickable);
}
