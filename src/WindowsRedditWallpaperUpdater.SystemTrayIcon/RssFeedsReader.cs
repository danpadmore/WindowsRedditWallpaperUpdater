using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using WindowsRedditWallpaperUpdater.Library;

namespace WindowsRedditWallpaperUpdater.SystemTrayIcon
{
    public class RssFeedsReader
    {
        public static List<RssFeed> ReadFileFromDisk(string rssFeedsFileOnDisk)
        {
            var rssFeeds = new List<RssFeed>();

            try
            {
                var jsonContentFromDisk = File.ReadAllText(rssFeedsFileOnDisk);
                var rssFeedFileContent = JsonConvert.DeserializeObject<RssFeedFileContent>(jsonContentFromDisk);
                rssFeeds = rssFeedFileContent.RssFeeds;
            }
            catch (FileNotFoundException)
            {
                EventLog.WriteEntry("Application", $"File {rssFeedsFileOnDisk} not found.", EventLogEntryType.Error);
            }
            catch (Exception exception)
            {
                EventLog.WriteEntry("Application", exception.ToString(), EventLogEntryType.Error);
            }

            return rssFeeds;
        }

        private struct RssFeedFileContent
        {
            public List<RssFeed> RssFeeds { get; set; }
        }
    }
}