using AsciiWallpaperEngine.Core.Interfaces;
using AsciiWallpaperEngine.Core.Models;
using AsciiWallpaperEngine.Scenes;
using System.IO;
using System.Text.Json;

namespace AsciiWallpaperEngine.Engine;

public sealed class SceneManager
{
    private readonly EngineConfig _config;
    private ISceneSource? _current;

    public SceneManager(EngineConfig config)
    {
        _config = config;
        if (_config.Themes.Count == 0)
        {
            _config.Themes.Add(new ThemeProfile { Name = "Default", SourceType = "static", SourcePath = string.Empty });
        }
    }

    public IReadOnlyList<ThemeProfile> Themes => _config.Themes;

    public void NextTheme()
    {
        if (_config.Themes.Count == 0)
        {
            return;
        }

        var idx = _config.Themes.FindIndex(t => t.Name == _config.SelectedThemeName);
        idx = idx < 0 ? 0 : (idx + 1) % _config.Themes.Count;
        _config.SelectedThemeName = _config.Themes[idx].Name;
    }

    public async ValueTask<ISceneSource> GetActiveSceneAsync()
    {
        var theme = _config.Themes.FirstOrDefault(t => t.Name == _config.SelectedThemeName) ?? _config.Themes[0];

        if (_current is not null)
        {
            await _current.DisposeAsync();
            _current = null;
        }

        _current = theme.SourceType.ToLowerInvariant() switch
        {
            "animated" => await CreateAnimatedScene(theme.SourcePath),
            "video" => new VideoAsciiScene(new VideoToAsciiConverter(), _config),
            _ => new StaticAsciiScene(theme.SourcePath)
        };

        return _current;
    }

    private static async Task<ISceneSource> CreateAnimatedScene(string path)
    {
        if (!File.Exists(path))
        {
            return new StaticAsciiScene(path);
        }

        await using var fs = File.OpenRead(path);
        var doc = await JsonSerializer.DeserializeAsync<AnimatedAsciiDocument>(fs);
        if (doc is null || doc.Frames.Count == 0)
        {
            return new StaticAsciiScene(path);
        }

        return new AnimatedAsciiScene(doc.Frames, doc.Fps);
    }
}
