using AsciiWallpaperEngine.Core.Interfaces;
using AsciiWallpaperEngine.Core.Models;
using System.IO;
using System.Threading.Channels;

namespace AsciiWallpaperEngine.Scenes;

public sealed class VideoAsciiScene : ISceneSource
{
    private readonly Channel<AsciiFrame> _channel = Channel.CreateBounded<AsciiFrame>(new BoundedChannelOptions(3)
    {
        FullMode = BoundedChannelFullMode.DropOldest,
        SingleReader = true,
        SingleWriter = true
    });

    private readonly CancellationTokenSource _cts = new();
    private readonly Task _producer;

    public VideoAsciiScene(VideoToAsciiConverter converter, EngineConfig config)
    {
        NativeFps = Math.Max(1, config.TargetFps);
        var path = config.VideoPath;

        _producer = Task.Run(async () =>
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                await _channel.Writer.WriteAsync(AsciiFrame.FromLines(["Видео не выбрано"]));
                _channel.Writer.TryComplete();
                return;
            }

            await foreach (var frame in converter.ReadAsciiFrames(path, config.GridWidth, config.GridHeight, config.Invert, config.ColorMode, (config.MonochromeR, config.MonochromeG, config.MonochromeB), _cts.Token))
            {
                if (!_channel.Writer.TryWrite(frame))
                {
                    await _channel.Writer.WriteAsync(frame, _cts.Token);
                }
            }

            _channel.Writer.TryComplete();
        }, _cts.Token);
    }

    public int NativeFps { get; }

    public async ValueTask<AsciiFrame?> GetNextFrameAsync(CancellationToken cancellationToken)
    {
        if (await _channel.Reader.WaitToReadAsync(cancellationToken) && _channel.Reader.TryRead(out var frame))
        {
            return frame;
        }

        return null;
    }

    public async ValueTask DisposeAsync()
    {
        _cts.Cancel();
        try
        {
            await _producer;
        }
        catch
        {
            // ignored
        }
        _cts.Dispose();
    }
}
