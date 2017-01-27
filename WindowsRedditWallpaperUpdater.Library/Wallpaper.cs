using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace WindowsRedditWallpaperUpdater.Library
{
    /// <summary>
    /// Based on http://stackoverflow.com/questions/1061678/change-desktop-wallpaper-using-code-in-net#1061682
    /// </summary>
    public sealed class Wallpaper
    {
        private const string WallpaperFilename = "wallpaper-updater.bmp";
        private const int SPI_SETDESKWALLPAPER = 20;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        Wallpaper()
        {
        }

        public static void Set(Uri uri, WallpaperStyle style)
        {
            var wallpaperPath = DownloadWallpaper(uri);

            SetWallpaperStyle(style);

            SystemParametersInfo(
                uAction: SPI_SETDESKWALLPAPER,
                uParam: 0,
                lpvParam: wallpaperPath,
                fuWinIni: SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

            WallpaperHistory.Add(uri);
        }

        private static string DownloadWallpaper(Uri uri)
        {
            using (var webclient = new WebClient())
            using (var wallpaperStream = webclient.OpenRead(uri.ToString()))
            using (var wallpaperImage = Image.FromStream(wallpaperStream))
            {
                var wallpaperPath = Path.Combine(Path.GetTempPath(), WallpaperFilename);
                wallpaperImage.Save(wallpaperPath, ImageFormat.Bmp);

                return wallpaperPath;
            }
        }

        private static void SetWallpaperStyle(WallpaperStyle style)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true))
            {
                switch (style)
                {
                    case WallpaperStyle.Tiled:
                        key.SetValue(@"WallpaperStyle", 1.ToString());
                        key.SetValue(@"TileWallpaper", 1.ToString());
                        break;

                    case WallpaperStyle.Centered:
                        key.SetValue(@"WallpaperStyle", 1.ToString());
                        key.SetValue(@"TileWallpaper", 0.ToString());
                        break;

                    case WallpaperStyle.Stretched:
                    default:
                        key.SetValue(@"WallpaperStyle", 2.ToString());
                        key.SetValue(@"TileWallpaper", 0.ToString());
                        break;
                }
            }
        }
    }
}