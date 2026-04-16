namespace AsciiWallpaperEngine.Core.Models;

public sealed class AsciiFrame
{
    public int Columns { get; }
    public int Rows { get; }
    public AsciiCell[] Cells { get; }

    public AsciiFrame(int columns, int rows, AsciiCell[] cells)
    {
        Columns = columns;
        Rows = rows;
        Cells = cells;
    }

    public static AsciiFrame FromLines(IReadOnlyList<string> lines)
    {
        var rows = lines.Count;
        var columns = lines.Count == 0 ? 0 : lines.Max(l => l.Length);
        var cells = new AsciiCell[Math.Max(1, rows * Math.Max(1, columns))];

        for (var y = 0; y < rows; y++)
        {
            var line = lines[y];
            for (var x = 0; x < columns; x++)
            {
                var ch = x < line.Length ? line[x] : ' ';
                cells[y * columns + x] = new AsciiCell { Char = ch, R = 255, G = 255, B = 255 };
            }
        }

        return new AsciiFrame(columns, rows, cells);
    }
}
