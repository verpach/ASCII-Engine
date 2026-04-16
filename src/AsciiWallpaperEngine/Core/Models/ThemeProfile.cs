namespace AsciiWallpaperEngine.Core.Models;

public sealed class ThemeProfile
{
    public string Name { get; set; } = "Default";
    public string SourcePath { get; set; } = string.Empty;
    public string SourceType { get; set; } = "static";
}
