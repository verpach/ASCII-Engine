using AsciiWallpaperEngine.Core.Interfaces;
using AsciiWallpaperEngine.Core.Models;
using System.IO;

namespace AsciiWallpaperEngine.Scenes;

public sealed class StaticAsciiScene : ISceneSource
{
    private readonly AsciiFrame _frame;

    public StaticAsciiScene(string filePath)
    {
        var lines = File.Exists(filePath) ? File.ReadAllLines(filePath) : ["ASCII Wallpaper Engine"];
        _frame = AsciiFrame.FromLines(lines);
    }

    public int NativeFps => 1;

    public ValueTask<AsciiFrame?> GetNextFrameAsync(CancellationToken cancellationToken)
        => ValueTask.FromResult<AsciiFrame?>(_frame);

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
