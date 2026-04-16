using AsciiWallpaperEngine.Core.Interfaces;
using AsciiWallpaperEngine.Core.Models;

namespace AsciiWallpaperEngine.Scenes;

public sealed class AnimatedAsciiScene : ISceneSource
{
    private readonly List<string[]> _frames;
    private readonly TimeSpan _frameDuration;
    private DateTime _nextAt = DateTime.MinValue;
    private int _index;
    private AsciiFrame? _last;

    public int NativeFps { get; }

    public AnimatedAsciiScene(List<string[]> frames, int fps)
    {
        _frames = frames;
        NativeFps = Math.Max(1, fps);
        _frameDuration = TimeSpan.FromSeconds(1.0 / NativeFps);
    }

    public ValueTask<AsciiFrame?> GetNextFrameAsync(CancellationToken cancellationToken)
    {
        if (_frames.Count == 0)
        {
            return ValueTask.FromResult<AsciiFrame?>(null);
        }

        if (_last is not null && DateTime.UtcNow < _nextAt)
        {
            return ValueTask.FromResult<AsciiFrame?>(_last);
        }

        var lines = _frames[_index];
        _index = (_index + 1) % _frames.Count;
        _last = AsciiFrame.FromLines(lines);
        _nextAt = DateTime.UtcNow.Add(_frameDuration);
        return ValueTask.FromResult<AsciiFrame?>(_last);
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
