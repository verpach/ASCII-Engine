using AsciiWallpaperEngine.Core.Models;

namespace AsciiWallpaperEngine.Rendering;

public sealed class AsciiConverter
{
    private const string Density = " .:-=+*#%@";

    public AsciiFrame Convert(byte[] bgra, int srcW, int srcH, int cols, int rows, bool invert, AsciiColorMode colorMode, (byte r, byte g, byte b) monoColor)
    {
        var cells = new AsciiCell[rows * cols];
        var cellW = Math.Max(1, srcW / cols);
        var cellH = Math.Max(1, srcH / rows);

        for (var y = 0; y < rows; y++)
        {
            for (var x = 0; x < cols; x++)
            {
                long sumLuma = 0;
                long sumR = 0;
                long sumG = 0;
                long sumB = 0;
                var count = 0;

                var sx0 = x * cellW;
                var sy0 = y * cellH;
                var sx1 = Math.Min(srcW, sx0 + cellW);
                var sy1 = Math.Min(srcH, sy0 + cellH);

                for (var py = sy0; py < sy1; py++)
                {
                    for (var px = sx0; px < sx1; px++)
                    {
                        var i = (py * srcW + px) * 4;
                        var b = bgra[i];
                        var g = bgra[i + 1];
                        var r = bgra[i + 2];
                        var luma = (r * 299 + g * 587 + b * 114) / 1000;
                        sumLuma += luma;
                        sumR += r;
                        sumG += g;
                        sumB += b;
                        count++;
                    }
                }

                var avg = count == 0 ? 0 : (int)(sumLuma / count);
                if (invert)
                {
                    avg = 255 - avg;
                }

                var index = (avg * (Density.Length - 1)) / 255;
                byte outR;
                byte outG;
                byte outB;

                switch (colorMode)
                {
                    case AsciiColorMode.Monochrome:
                        outR = monoColor.r;
                        outG = monoColor.g;
                        outB = monoColor.b;
                        break;
                    case AsciiColorMode.ByBrightness:
                        outR = (byte)avg;
                        outG = (byte)avg;
                        outB = (byte)avg;
                        break;
                    default:
                        outR = count == 0 ? (byte)0 : (byte)(sumR / count);
                        outG = count == 0 ? (byte)0 : (byte)(sumG / count);
                        outB = count == 0 ? (byte)0 : (byte)(sumB / count);
                        break;
                }

                cells[y * cols + x] = new AsciiCell
                {
                    Char = Density[index],
                    R = outR,
                    G = outG,
                    B = outB
                };
            }
        }

        return new AsciiFrame(cols, rows, cells);
    }
}
