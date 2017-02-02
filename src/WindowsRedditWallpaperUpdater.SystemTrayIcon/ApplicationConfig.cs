using System;
using System.Diagnostics;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using WindowsRedditWallpaperUpdater.Library;

namespace WindowsRedditWallpaperUpdater.SystemTrayIcon
{
    public class ApplicationConfig : ApplicationContext
    {
        private readonly NotifyIcon _notifyIcon;
        private readonly WallpaperUpdater _wallpaperUpdater;
        private readonly System.Timers.Timer _wallpaperRefreshTimer;

        private static string RssUrl { get { return Properties.Settings.Default.RssUrl; } }

        public ApplicationConfig(NotifyIcon notifyIcon)
        {
            if (notifyIcon == null) throw new ArgumentNullException(nameof(notifyIcon));

            _notifyIcon = notifyIcon;
            _wallpaperUpdater = new WallpaperUpdater();
            _wallpaperRefreshTimer = new System.Timers.Timer();

            InitializeTimer();
            InitializeTrayIcon();
            Application.ApplicationExit += new EventHandler(OnApplicationExit);

            _wallpaperUpdater.Update(RssUrl);
        }

        private void InitializeTimer()
        {
            _wallpaperRefreshTimer.Elapsed += new ElapsedEventHandler(NextWallpaperEvent);
            _wallpaperRefreshTimer.Interval = ((Properties.Settings.Default.TimerIntervalInMinutes * 60) * 1000);
            _wallpaperRefreshTimer.Start();
        }

        private void InitializeTrayIcon()
        {
            _notifyIcon.Text = "WindowsRedditWallpaperUpdater";
            _notifyIcon.Icon = Properties.Resources.IconEarth;

            _notifyIcon.ContextMenuStrip = BuildContextMenu(new ToolStripItem[] {
                BuildToolStripMenuItem("Next Wallpaper", new EventHandler(NextWallpaperEvent)),
                BuildToolStripMenuItem("Exit", new EventHandler(ExitEvent))
            });

            _notifyIcon.Visible = true;
        }

        private ContextMenuStrip BuildContextMenu(ToolStripItem[] toolStripMenuItem)
        {
            var contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.AddRange(toolStripMenuItem);
            contextMenuStrip.Name = "TrayIconContextMenu";
            contextMenuStrip.Size = new Size(153, 70);

            return contextMenuStrip;
        }

        private ToolStripMenuItem BuildToolStripMenuItem(string name, EventHandler eventHandler)
        {
            var toolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem.Name = name.Replace(" ", "");
            toolStripMenuItem.Size = new Size(152, 22);
            toolStripMenuItem.Text = name;
            toolStripMenuItem.Click += eventHandler;

            return toolStripMenuItem;
        }

        private void NextWallpaperEvent(object sender, EventArgs e)
        {
            _wallpaperUpdater.Update(RssUrl);
            _wallpaperRefreshTimer.Stop();
            _wallpaperRefreshTimer.Start();
        }

        private void ExitEvent(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
            EventLog.WriteEntry("Application", $"WindowsRedditWallpaperUpdater.SystemTrayIcon exited", EventLogEntryType.Information);
        }
    }
}
