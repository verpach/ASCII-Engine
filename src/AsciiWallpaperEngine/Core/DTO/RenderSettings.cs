using AsciiWallpaperEngine.Core.Models;
using MediaColor = System.Windows.Media.Color;
using MediaColors = System.Windows.Media.Colors;

namespace AsciiWallpaperEngine.Core.DTO;

public sealed class RenderSettings
{
    public int Width { get; init; }
    public int Height { get; init; }
    public string FontFamily { get; init; } = "Consolas";
    public double FontSize { get; init; } = 10;
    public MediaColor Background { get; init; } = MediaColors.Black;
    public AsciiColorMode ColorMode { get; init; } = AsciiColorMode.Monochrome;
    public MediaColor MonochromeColor { get; init; } = MediaColors.White;
}
