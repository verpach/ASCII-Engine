using AsciiWallpaperEngine.Core.Models;

namespace AsciiWallpaperEngine.Core.Interfaces;

public interface ISceneSource : IAsyncDisposable
{
    int NativeFps { get; }
    ValueTask<AsciiFrame?> GetNextFrameAsync(CancellationToken cancellationToken);
}
