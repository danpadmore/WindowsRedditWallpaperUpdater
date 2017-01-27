using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;
using static System.Console;

namespace WindowsRedditWallpaperUpdater.Console
{
    class Program
    {
        private static readonly Regex WallpaperLinkRegex = new Regex(@"<a href=""([^ ""]*[.jpg|.png])"">\[link");
        public static string RssUrl { get { return ConfigurationManager.AppSettings["rssUrl"]; } }

        static void Main(string[] args)
        {
            try
            {
                WriteLine("Begin");

                using (var xmlReader = XmlReader.Create(RssUrl))
                {
                    var feed = SyndicationFeed.Load(xmlReader);
                    var feedItems = feed.Items
                        .OrderByDescending(i => i.PublishDate)
                        .Skip(1)
                        .Select(i => (i.Content as TextSyndicationContent).Text);

                    foreach (var feedItem in feedItems)
                    {
                        var wallpaperUrl = WallpaperLinkRegex.Match(feedItem).Groups[1]?.Value;
                        if (string.IsNullOrWhiteSpace(wallpaperUrl))
                            continue;

                        if (WallpaperHistory.IsKnown(wallpaperUrl))
                            continue;

                        try
                        {
                            Wallpaper.Set(new Uri(wallpaperUrl), WallpaperStyle.Stretched);
                            EventLog.WriteEntry("Application", $"WallpaperUpdater succeeded: changed to '{wallpaperUrl}'", EventLogEntryType.Information);
                            break;
                        }
                        catch (Exception ex)
                        {
                            WallpaperHistory.Ignore(wallpaperUrl);
                            EventLog.WriteEntry("Application", $"WallpaperUpdater ignored '{wallpaperUrl}': {ex}", EventLogEntryType.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLine(ex);
                EventLog.WriteEntry("Application", $"WallpaperUpdater failed: {ex}", EventLogEntryType.Information);
            }
            finally
            {
                WriteLine("End");
            }
        }
    }
}
