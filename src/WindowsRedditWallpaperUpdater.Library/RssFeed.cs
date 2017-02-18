using System;

namespace WindowsRedditWallpaperUpdater.Library
{
    public class RssFeed
    {
        private string name;
        private string url;

        public RssFeed(string name, string url)
        {
            this.name = name;
            this.url = url;
        }

        public string Name { get { return name; } set { name = value; } }
        public string Url { get { return url; } set { url = value; } }
    }
}