using System;
using System.Configuration;
using System.Diagnostics;
using WindowsRedditWallpaperUpdater.Library;
using static System.Console;

namespace WindowsRedditWallpaperUpdater.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                WriteLine("Begin");

                var wallpaperUpdater = new WallpaperUpdater();
                var rssUrl = ConfigurationManager.AppSettings["rssUrl"];

                wallpaperUpdater.Update(new RssFeed("Default feed", rssUrl));
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