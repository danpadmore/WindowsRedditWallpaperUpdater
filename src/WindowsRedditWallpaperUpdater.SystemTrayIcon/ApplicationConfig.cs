using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using WindowsRedditWallpaperUpdater.Library;

namespace WindowsRedditWallpaperUpdater.SystemTrayIcon
{
    public class ApplicationConfig : ApplicationContext
    {
        private readonly int intervalInMinutes = int.Parse(ConfigurationManager.AppSettings["refreshIntervalInMinutes"]);
        private readonly string rssUrl = ConfigurationManager.AppSettings["rssUrl"];

        private readonly WallpaperUpdater wallpaperUpdater;
        private readonly IntervalTimer intervalTimer;
        private readonly TrayIcon trayIcon;

        public ApplicationConfig()
        {
            wallpaperUpdater = new WallpaperUpdater();
            intervalTimer = new IntervalTimer(() => wallpaperUpdater.Update(rssUrl), intervalInMinutes);

            trayIcon = new TrayIcon("WindowsRedditWallpaperUpdater", Properties.Resources.SystemTrayIcon)
                .AddMenuItem("Next Wallpaper", new EventHandler(OnNextWallpaper))
                .AddMenuItem("Exit", new EventHandler(OnExit))
                .Initialize();

            Application.ApplicationExit += new EventHandler(OnApplicationExit);

            wallpaperUpdater.Update(rssUrl);
        }

        private void OnNextWallpaper(object sender, EventArgs e)
        {
            wallpaperUpdater.Update(rssUrl);
            intervalTimer.RestartInterval();
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            trayIcon.Dispose();
        }
    }
}