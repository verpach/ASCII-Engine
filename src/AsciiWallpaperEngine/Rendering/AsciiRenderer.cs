using AsciiWallpaperEngine.Core.DTO;
using AsciiWallpaperEngine.Core.Interfaces;
using AsciiWallpaperEngine.Core.Models;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AsciiWallpaperEngine.Rendering;

public sealed class AsciiRenderer : IRenderer
{
    private readonly GlyphAtlas _glyphAtlas = new();

    public WriteableBitmap Render(AsciiFrame frame, RenderSettings settings)
    {
        var visual = new DrawingVisual();
        using (var ctx = visual.RenderOpen())
        {
            ctx.DrawRectangle(new SolidColorBrush(settings.Background), null, new Rect(0, 0, settings.Width, settings.Height));

            if (frame.Columns <= 0 || frame.Rows <= 0)
            {
                var empty = new RenderTargetBitmap(settings.Width, settings.Height, 96, 96, PixelFormats.Pbgra32);
                empty.Render(visual);
                return new WriteableBitmap(empty);
            }

            var cellW = settings.Width / (double)frame.Columns;
            var cellH = settings.Height / (double)frame.Rows;

            for (var y = 0; y < frame.Rows; y++)
            {
                for (var x = 0; x < frame.Columns; x++)
                {
                    var cell = frame.Cells[y * frame.Columns + x];
                    var color = System.Windows.Media.Color.FromRgb(cell.R, cell.G, cell.B);
                    var glyph = _glyphAtlas.Get(cell.Char, color, settings.FontFamily, settings.FontSize);
                    ctx.DrawText(glyph, new System.Windows.Point(x * cellW, y * cellH));
                }
            }
        }

        var rtb = new RenderTargetBitmap(settings.Width, settings.Height, 96, 96, PixelFormats.Pbgra32);
        rtb.Render(visual);
        return new WriteableBitmap(rtb);
    }
}
