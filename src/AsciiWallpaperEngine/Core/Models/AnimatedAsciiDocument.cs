namespace AsciiWallpaperEngine.Core.Models;

public sealed class AnimatedAsciiDocument
{
    public int Fps { get; set; } = 24;
    public List<string[]> Frames { get; set; } = [];
}
