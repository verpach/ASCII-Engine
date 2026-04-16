namespace AsciiWallpaperEngine.Core.Models;

public sealed class EngineConfig
{
    public bool Enabled { get; set; } = true;
    public int GridWidth { get; set; } = 180;
    public int GridHeight { get; set; } = 100;
    public string FontFamily { get; set; } = "Consolas";
    public double FontSize { get; set; } = 10;
    public bool Invert { get; set; }
    public bool BlurEnabled { get; set; }
    public AsciiColorMode ColorMode { get; set; } = AsciiColorMode.ByPixelColor;
    public byte MonochromeR { get; set; } = 255;
    public byte MonochromeG { get; set; } = 255;
    public byte MonochromeB { get; set; } = 255;
    public int TargetFps { get; set; } = 30;
    public bool PauseOnFullscreen { get; set; } = true;
    public bool PauseOnBattery { get; set; }
    public bool AutoStart { get; set; }
    public bool StartInTray { get; set; }
    public bool Clickable { get; set; }
    public string? VideoPath { get; set; }
    public string SelectedThemeName { get; set; } = "Default";
    public List<ThemeProfile> Themes { get; set; } = [];
}
