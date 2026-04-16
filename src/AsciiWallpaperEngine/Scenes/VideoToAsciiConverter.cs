using AsciiWallpaperEngine.Core.Models;
using AsciiWallpaperEngine.Rendering;
using OpenCvSharp;
using System.Runtime.CompilerServices;

namespace AsciiWallpaperEngine.Scenes;

public sealed class VideoToAsciiConverter
{
    private readonly AsciiConverter _converter = new();

    public async IAsyncEnumerable<AsciiFrame> ReadAsciiFrames(
        string path,
        int cols,
        int rows,
        bool invert,
        AsciiColorMode colorMode,
        (byte r, byte g, byte b) monoColor,
        [EnumeratorCancellation] CancellationToken ct)
    {
        using var cap = new VideoCapture(path);
        using var mat = new Mat();

        while (!ct.IsCancellationRequested && cap.Read(mat))
        {
            using var bgra = new Mat();
            Cv2.CvtColor(mat, bgra, ColorConversionCodes.BGR2BGRA);
            var bytes = new byte[bgra.Rows * bgra.Cols * 4];
            System.Runtime.InteropServices.Marshal.Copy(bgra.Data, bytes, 0, bytes.Length);

            yield return _converter.Convert(bytes, bgra.Cols, bgra.Rows, cols, rows, invert, colorMode, monoColor);
            await Task.Yield();
        }
    }
}
