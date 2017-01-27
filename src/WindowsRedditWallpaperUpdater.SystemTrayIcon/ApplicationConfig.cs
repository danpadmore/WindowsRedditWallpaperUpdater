using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using WindowsRedditWallpaperUpdater.Library;

namespace WindowsRedditWallpaperUpdater.SystemTrayIcon
{
    public class ApplicationConfig : ApplicationContext
    {
        private System.Timers.Timer wallpaperRefreshTimer;
        private NotifyIcon NotifyIcon;
        private static string RssUrl { get { return Properties.Settings.Default.RssUrl; } }

        public ApplicationConfig()
        {
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            FetchWallpaperOnApplicationInit();
            InitializeTrayIcon();
            InitializeTimer();
        }

        private void FetchWallpaperOnApplicationInit()
        {
            WallpaperFetcher.FetchAndSet(RssUrl);
        }

        private void InitializeTimer()
        {
            wallpaperRefreshTimer = new System.Timers.Timer();
            wallpaperRefreshTimer.Elapsed += new ElapsedEventHandler(NextWallpaperEvent);
            wallpaperRefreshTimer.Interval = ((Properties.Settings.Default.TimerIntervalInMinutes * 60) * 1000);
            wallpaperRefreshTimer.Start();
        }

        private void InitializeTrayIcon()
        {
            NotifyIcon = new NotifyIcon();
            NotifyIcon.Text = "WindowsRedditWallpaperUpdater";
            NotifyIcon.Icon = Properties.Resources.IconEarth;

            NotifyIcon.ContextMenuStrip = BuildContextMenu(new ToolStripItem[] {
                BuildToolStripMenuItem("Next Wallpaper", new EventHandler(NextWallpaperEvent)),
                BuildToolStripMenuItem("Exit", new EventHandler(ExitEvent))
            });

            NotifyIcon.Visible = true;
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

        private void OnApplicationExit(object sender, EventArgs e)
        {
            NotifyIcon.Visible = false;
        }

        private void NextWallpaperEvent(object sender, EventArgs e)
        {
            WallpaperFetcher.FetchAndSet(RssUrl);
            wallpaperRefreshTimer.Stop();
            wallpaperRefreshTimer.Start();
        }

        private void ExitEvent(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
