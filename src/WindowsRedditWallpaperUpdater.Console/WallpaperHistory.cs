using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace WindowsRedditWallpaperUpdater.Console
{
    public class WallpaperHistory
    {
        private static readonly Lazy<List<string>> _cachedWallpaperHistory;

        static WallpaperHistory()
        {
            _cachedWallpaperHistory = new Lazy<List<string>>(() => 
            {
                using (File.Open(WallpaperHistoryPath, FileMode.OpenOrCreate)) { }
                return File.ReadLines(WallpaperHistoryPath).ToList();
            });
        }

        private static string WallpaperHistoryPath
        {
            get { return ConfigurationManager.AppSettings["wallpaperHistoryPath"]; }
        }

        public static void Add(Uri wallpaperUri)
        {
            File.AppendAllText(WallpaperHistoryPath, $"{wallpaperUri.AbsoluteUri}{Environment.NewLine}");
        }

        public static void Ignore(string wallpaperUrl)
        {
            File.AppendAllText(WallpaperHistoryPath, $"{wallpaperUrl}{Environment.NewLine}");
        }

        public static bool IsKnown(string wallpaperUri)
        {
            if (string.IsNullOrWhiteSpace(wallpaperUri))
                return false;

            return _cachedWallpaperHistory.Value.Any(h => h == wallpaperUri);
        }
    }
}
