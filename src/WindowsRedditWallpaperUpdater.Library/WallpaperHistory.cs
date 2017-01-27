using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WindowsRedditWallpaperUpdater.Library
{
    public class WallpaperHistory
    {
        private const string HistoryFileName = "WindowsRedditWallpaperUpdater-history.txt";
        private static List<string> wallpaperHistory;

        static WallpaperHistory()
        {
            CreateFile();
            RefreshHistory();
        }

        private static void CreateFile()
        {
            using (File.Open(WallpaperHistoryPath, FileMode.OpenOrCreate)) { }
        }

        private static void RefreshHistory()
        {
            wallpaperHistory = File.ReadLines(WallpaperHistoryPath).ToList();
        }

        private static string WallpaperHistoryPath
        {
            get { return Path.Combine(Path.GetTempPath(), HistoryFileName); }
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

            RefreshHistory();

            return wallpaperHistory.Any(h => h == wallpaperUri);
        }
    }
}
