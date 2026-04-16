using AsciiWallpaperEngine.Core.DTO;
using AsciiWallpaperEngine.Core.Interfaces;
using AsciiWallpaperEngine.Core.Models;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AsciiWallpaperEngine.Engine;

public sealed class AsciiEngine
{
    private readonly SceneManager _sceneManager;
    private readonly IRenderer _renderer;
    private readonly PerformanceModeService _performanceMode;
    private readonly EngineConfig _config;
    private readonly FpsLimiter _fpsLimiter = new();
    private readonly CancellationTokenSource _cts = new();

    private Task? _runner;

    public event Action<WriteableBitmap>? FrameReady;

    public AsciiEngine(SceneManager sceneManager, IRenderer renderer, PerformanceModeService performanceMode, EngineConfig config)
    {
        _sceneManager = sceneManager;
        _renderer = renderer;
        _performanceMode = performanceMode;
        _config = config;
    }

    public Task StartAsync()
    {
        _runner ??= RunLoopAsync(_cts.Token);
        return _runner;
    }

    public async Task StopAsync()
    {
        _cts.Cancel();
        if (_runner is not null)
        {
            try
            {
                await _runner;
            }
            catch (OperationCanceledException)
            {
                // expected
            }
        }

        _cts.Dispose();
    }

    private async Task RunLoopAsync(CancellationToken cancellationToken)
    {
        var scene = await _sceneManager.GetActiveSceneAsync();

        while (!cancellationToken.IsCancellationRequested)
        {
            if (!_config.Enabled || _performanceMode.ShouldPause())
            {
                await Task.Delay(250, cancellationToken);
                continue;
            }

            var frame = await scene.GetNextFrameAsync(cancellationToken);
            if (frame is not null)
            {
                var bitmap = _renderer.Render(frame, new RenderSettings
                {
                    Width = 1920,
                    Height = 1080,
                    FontFamily = _config.FontFamily,
                    FontSize = _config.FontSize,
                    Background = Colors.Black,
                    ColorMode = _config.ColorMode,
                    MonochromeColor = System.Windows.Media.Color.FromRgb(_config.MonochromeR, _config.MonochromeG, _config.MonochromeB)
                });

                FrameReady?.Invoke(bitmap);
            }

            await _fpsLimiter.DelayToTargetAsync(_config.TargetFps, cancellationToken);
        }

        await scene.DisposeAsync();
    }
}
