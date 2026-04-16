using System.Drawing;
using System.Windows.Forms;

namespace AsciiWallpaperEngine.Infrastructure.Tray;

public sealed class TrayService : IDisposable
{
    private NotifyIcon? _notifyIcon;

    public void Initialize(Action onShow, Action onExit)
    {
        _notifyIcon = new NotifyIcon
        {
            Icon = SystemIcons.Application,
            Visible = true,
            Text = "ASCII Wallpaper Engine"
        };

        var menu = new ContextMenuStrip();
        menu.Items.Add("Открыть", null, (_, _) => onShow());
        menu.Items.Add("Выход", null, (_, _) => onExit());
        _notifyIcon.ContextMenuStrip = menu;
        _notifyIcon.DoubleClick += (_, _) => onShow();
    }

    public void Dispose()
    {
        if (_notifyIcon is null)
        {
            return;
        }

        _notifyIcon.Visible = false;
        _notifyIcon.Dispose();
        _notifyIcon = null;
    }
}
