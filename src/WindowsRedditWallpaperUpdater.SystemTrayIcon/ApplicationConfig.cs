using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using WindowsRedditWallpaperUpdater.Library;

namespace WindowsRedditWallpaperUpdater.SystemTrayIcon
{
    public class ApplicationConfig : ApplicationContext
    {
        private readonly string rssFeedsFile = ConfigurationManager.AppSettings["rssFeedsFile"];
        private readonly int intervalInMinutes = int.Parse(ConfigurationManager.AppSettings["refreshIntervalInMinutes"]);

        private readonly WallpaperUpdater wallpaperUpdater;
        private readonly IntervalTimer intervalTimer;
        private readonly TrayIcon trayIcon;
        private List<RssFeed> rssFeeds;
        private RssFeed selectedRssFeed;

        public ApplicationConfig()
        {
            ReadRssFeedsFromFileOnDisk();

            trayIcon = new TrayIcon("WindowsRedditWallpaperUpdater", Properties.Resources.SystemTrayIcon);
            AddRssFeedsAsMenuItems();
            AddDefaultMenuItems();
            trayIcon.Initialize();

            intervalTimer = new IntervalTimer(() => wallpaperUpdater.Update(selectedRssFeed), intervalInMinutes);
            Application.ApplicationExit += new EventHandler(OnApplicationExit);

            wallpaperUpdater = new WallpaperUpdater();
            wallpaperUpdater.Update(selectedRssFeed);
        }

        private void ReadRssFeedsFromFileOnDisk()
        {
            var rssFeedsPath = Path.IsPathRooted(rssFeedsFile)
                ? rssFeedsFile
                : Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, rssFeedsFile);
            rssFeeds = RssFeedsReader.ReadFileFromDisk(rssFeedsPath);

            if (rssFeeds.Count < 1)
                EventLog.WriteEntry("Application", $"No rss feeds found in file {rssFeedsPath}.", EventLogEntryType.Error);

            selectedRssFeed = rssFeeds.First();
        }

        private void AddDefaultMenuItems()
        {
            trayIcon
                .AddMenuItem("Next Wallpaper", new EventHandler(OnNextWallpaper))
                .AddMenuItem("Exit", new EventHandler(OnExit));
        }

        private void AddRssFeedsAsMenuItems()
        {
            foreach (var rssFeed in rssFeeds)
            {
                trayIcon.AddMenuItem(rssFeed.Name, new EventHandler(OnNextWallpaper));
            }
        }

        private void OnNextWallpaper(object sender, EventArgs e)
        {
            var menuItemName = sender.ToString();
            var nextRssFeed = DetermineNextRssFeed(menuItemName);

            DeleteHistoryIfFeedIsChangedAndSetNewFeed(nextRssFeed);

            wallpaperUpdater.Update(selectedRssFeed);
            intervalTimer.ResetInterval();
        }

        private void DeleteHistoryIfFeedIsChangedAndSetNewFeed(RssFeed nextRssFeed)
        {
            if (selectedRssFeed != nextRssFeed)
                WallpaperHistory.Delete();

            selectedRssFeed = nextRssFeed;
        }

        private RssFeed DetermineNextRssFeed(string menuItemName)
        {
            var foundRssFeed = rssFeeds.Where(f => f.Name.Equals(menuItemName)).FirstOrDefault();
            return foundRssFeed ?? selectedRssFeed;
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