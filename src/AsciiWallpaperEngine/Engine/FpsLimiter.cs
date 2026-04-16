namespace AsciiWallpaperEngine.Engine;

public sealed class FpsLimiter
{
    private DateTime _lastTick = DateTime.UtcNow;

    public async Task DelayToTargetAsync(int fps, CancellationToken cancellationToken)
    {
        fps = Math.Max(1, fps);
        var target = TimeSpan.FromSeconds(1.0 / fps);
        var now = DateTime.UtcNow;
        var elapsed = now - _lastTick;
        var delay = target - elapsed;
        if (delay > TimeSpan.Zero)
        {
            await Task.Delay(delay, cancellationToken);
        }

        _lastTick = DateTime.UtcNow;
    }
}
