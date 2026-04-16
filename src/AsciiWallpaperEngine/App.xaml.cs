using AsciiWallpaperEngine.Core.Models;
using AsciiWallpaperEngine.Engine;
using AsciiWallpaperEngine.Infrastructure.Config;
using AsciiWallpaperEngine.Infrastructure.Logging;
using AsciiWallpaperEngine.Infrastructure.Startup;
using AsciiWallpaperEngine.Infrastructure.Tray;
using AsciiWallpaperEngine.Infrastructure.WinApi;
using AsciiWallpaperEngine.Rendering;
using AsciiWallpaperEngine.ViewModels;
using AsciiWallpaperEngine.Views;
using Serilog;
using System.Windows;
using System.Windows.Forms;

namespace AsciiWallpaperEngine;

public partial class App : System.Windows.Application
{
    public static AppServices Services { get; private set; } = null!;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var log = LogBootstrapper.Configure();
        var configManager = new ConfigManager();
        var config = await configManager.LoadAsync();
        var autoStart = new AutoStartService();
        var renderer = new AsciiRenderer();
        var sceneManager = new SceneManager(config);
        var performanceMode = new PerformanceModeService(config);
        var engine = new AsciiEngine(sceneManager, renderer, performanceMode, config);
        var wallpaperHost = new WallpaperHost();
        var trayService = new TrayService();

        Services = new AppServices(log, configManager, autoStart, renderer, sceneManager, performanceMode, engine, wallpaperHost, trayService, config);

        var wallpaperWindows = new List<WallpaperWindow>();
        foreach (var screen in Screen.AllScreens)
        {
            var bounds = screen.Bounds;
            var window = new WallpaperWindow
            {
                Left = bounds.Left,
                Top = bounds.Top,
                Width = bounds.Width,
                Height = bounds.Height
            };
            wallpaperWindows.Add(window);
            window.Show();
        }

        engine.FrameReady += bitmap =>
        {
            foreach (var window in wallpaperWindows)
            {
                window.UpdateBitmap(bitmap);
            }
        };

        var mainVm = new MainViewModel(Services);
        var mainWindow = new MainWindow { DataContext = mainVm };
        MainWindow = mainWindow;
        mainWindow.Show();

        trayService.Initialize(() => Dispatcher.Invoke(mainWindow.Show), () => Dispatcher.Invoke(Shutdown));
        autoStart.SetAutoStart(config.AutoStart, Environment.ProcessPath ?? string.Empty);

        _ = engine.StartAsync();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (Services is not null)
        {
            await Services.Engine.StopAsync();
            await Services.ConfigManager.SaveAsync(Services.Config);
            Services.TrayService.Dispose();
            Log.CloseAndFlush();
        }

        base.OnExit(e);
    }
}

public sealed record AppServices(
    ILogger Logger,
    ConfigManager ConfigManager,
    AutoStartService AutoStartService,
    AsciiRenderer Renderer,
    SceneManager SceneManager,
    PerformanceModeService PerformanceMode,
    AsciiEngine Engine,
    WallpaperHost WallpaperHost,
    TrayService TrayService,
    EngineConfig Config);
