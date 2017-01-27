using System;
using System.Configuration;
using System.Diagnostics;
using WindowsRedditWallpaperUpdater.Library;
using static System.Console;

namespace WindowsRedditWallpaperUpdater.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                WriteLine("Begin");
        
                var wallpaperUpdater = new WallpaperUpdater();
                var rssUrl = ConfigurationManager.AppSettings["rssUrl"];

                wallpaperUpdater.Update(rssUrl);
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
