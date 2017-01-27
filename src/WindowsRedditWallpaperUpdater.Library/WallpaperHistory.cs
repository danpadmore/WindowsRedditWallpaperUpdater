using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;

namespace WindowsRedditWallpaperUpdater.Library
{
    public class WallpaperHistory
    {
        private const string WallpaperHistoryFilePath = "wallpaper-history.log";

        public void Add(Uri wallpaperUri)
        {
            using (var file = OpenFile(FileMode.Append))
            using (var writer = new StreamWriter(file))
            {
                writer.WriteLine($"{wallpaperUri.AbsoluteUri}{Environment.NewLine}");
            }
        }

        public void Ignore(string wallpaperUrl)
        {
            using (var file = OpenFile(FileMode.Append))
            using (var writer = new StreamWriter(file))
            {
                writer.WriteLine($"{wallpaperUrl}{Environment.NewLine}");
            }
        }

        public IReadOnlyList<string> GetAll()
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            using (var file = OpenFile(FileMode.OpenOrCreate))
            using (var reader = new StreamReader(file))
            {
                return reader.ReadToEnd()
                    .Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
            }
        }

        private IsolatedStorageFileStream OpenFile(FileMode fileMode)
        {
            return IsolatedStorageFile
                .GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null)
                .OpenFile(WallpaperHistoryFilePath, fileMode);
        }
    }
}
