using AsciiWallpaperEngine.Core.Models;
using System.IO;
using System.Text.Json;

namespace AsciiWallpaperEngine.Infrastructure.Config;

public sealed class ConfigManager
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public string ConfigDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AsciiWallpaperEngine");
    public string ConfigPath => Path.Combine(ConfigDirectory, "config.json");

    public async Task<EngineConfig> LoadAsync()
    {
        if (!File.Exists(ConfigPath))
        {
            var config = CreateDefaultConfig();
            await SaveAsync(config);
            return config;
        }

        await using var stream = File.OpenRead(ConfigPath);
        var cfg = await JsonSerializer.DeserializeAsync<EngineConfig>(stream, JsonOptions);
        return cfg ?? CreateDefaultConfig();
    }

    public async Task SaveAsync(EngineConfig config)
    {
        Directory.CreateDirectory(ConfigDirectory);
        await using var stream = File.Create(ConfigPath);
        await JsonSerializer.SerializeAsync(stream, config, JsonOptions);
    }

    private static EngineConfig CreateDefaultConfig() => new()
    {
        Themes =
        [
            new ThemeProfile
            {
                Name = "Default",
                SourceType = "static",
                SourcePath = Path.Combine(AppContext.BaseDirectory, "Assets", "default.txt")
            }
        ]
    };
}
