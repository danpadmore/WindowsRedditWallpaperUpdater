using System;
using System.Windows.Forms;

namespace WindowsRedditWallpaperUpdater.SystemTrayIcon
{
    static class Program
    {
        private static readonly NotifyIcon _notifyIcon;

        static Program()
        {
            _notifyIcon = new NotifyIcon();
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new ApplicationConfig(_notifyIcon));
        }
    }
}
