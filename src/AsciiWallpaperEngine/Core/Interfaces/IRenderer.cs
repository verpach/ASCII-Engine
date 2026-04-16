using AsciiWallpaperEngine.Core.DTO;
using AsciiWallpaperEngine.Core.Models;
using System.Windows.Media.Imaging;

namespace AsciiWallpaperEngine.Core.Interfaces;

public interface IRenderer
{
    WriteableBitmap Render(AsciiFrame frame, RenderSettings settings);
}
