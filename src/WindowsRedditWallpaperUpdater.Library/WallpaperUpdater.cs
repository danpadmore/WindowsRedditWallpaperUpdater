using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;

namespace WindowsRedditWallpaperUpdater.Library
{
    public class WallpaperUpdater
    {
        private readonly Wallpaper _wallpaper;
        private readonly WallpaperHistory _wallpaperHistory;
        private static readonly Regex WallpaperLinkRegex = new Regex(@"<a href=""([^ ""]*[.jpg|.png])"">\[link");

        public WallpaperUpdater()
        {
            _wallpaper = new Wallpaper();
            _wallpaperHistory = new WallpaperHistory();
        }

        public void Update(RssFeed rssFeed)
        {
            if (string.IsNullOrWhiteSpace(rssFeed.Url)) throw new ArgumentNullException(nameof(rssFeed.Url));

            var nextWallpaperUrl = GetNextWallpaperUrl(rssFeed.Url);

            try
            {
                _wallpaper.Set(new Uri(nextWallpaperUrl), WallpaperStyle.Stretched);

                EventLog.WriteEntry("Application", $"WallpaperUpdater succeeded: changed to '{nextWallpaperUrl}'", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                _wallpaperHistory.Ignore(nextWallpaperUrl);

                EventLog.WriteEntry("Application", $"WallpaperUpdater ignored '{nextWallpaperUrl}': {ex}", EventLogEntryType.Information);
            }
        }

        private string GetNextWallpaperUrl(string rssUrl)
        {
            using (var xmlReader = XmlReader.Create(rssUrl))
            {
                var feed = SyndicationFeed.Load(xmlReader);
                var feedItems = feed.Items
                    .OrderByDescending(i => i.PublishDate)
                    .Skip(1)
                    .Select(i => (i.Content as TextSyndicationContent).Text);

                var pastWallpapers = _wallpaperHistory.GetAll();
                var nextWallpaperUrl = feedItems
                    .Select(f => WallpaperLinkRegex.Match(f).Groups[1]?.Value)
                    .Where(w => !string.IsNullOrWhiteSpace(w) && !pastWallpapers.Any(p => p == w))
                    .FirstOrDefault();

                return nextWallpaperUrl;
            }
        }
    }
}