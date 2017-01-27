using System;
using System.Configuration;
using System.Diagnostics;
using WindowsRedditWallpaperUpdater.Library;
using static System.Console;

namespace WindowsRedditWallpaperUpdater.Console
{
    class Program
    {
        public static string RssUrl { get { return ConfigurationManager.AppSettings["rssUrl"]; } }

        static void Main(string[] args)
        {
            try
            {
                WriteLine("Begin");

                WallpaperFetcher.FetchAndSet(RssUrl);
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
