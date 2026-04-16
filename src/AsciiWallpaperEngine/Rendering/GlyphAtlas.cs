using System.Globalization;
using System.Windows;
using System.Windows.Media;
using MediaColor = System.Windows.Media.Color;

namespace AsciiWallpaperEngine.Rendering;

public sealed class GlyphAtlas
{
    private readonly Dictionary<(char ch, byte r, byte g, byte b, string font, double size), FormattedText> _cache = new();

    public FormattedText Get(char ch, MediaColor color, string fontFamily, double fontSize)
    {
        var key = (ch, color.R, color.G, color.B, fontFamily, fontSize);
        if (_cache.TryGetValue(key, out var value))
        {
            return value;
        }

        var text = new FormattedText(
            ch.ToString(),
            CultureInfo.InvariantCulture,
            System.Windows.FlowDirection.LeftToRight,
            new Typeface(new System.Windows.Media.FontFamily(fontFamily), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
            fontSize,
            new SolidColorBrush(color),
            1.0);

        _cache[key] = text;
        return text;
    }
}
