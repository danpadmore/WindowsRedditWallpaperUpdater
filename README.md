## Your favourite images subreddit on your desktop

This first version updates your wallpaper periodically to the most popular image [reddit's EarthPorn](https://www.reddit.com/r/EarthPorn/).

Thanks to [github.com/petermarnef](https://github.com/petermarnef) for turning the original console application into a more user friendly system tray icon.

### Install

Download all *WindowsRedditWallpaperUpdater.\** files from [the latest release](https://github.com/danpadmore/WindowsRedditWallpaperUpdater/releases) to a single folder on your computer. 

To run the application execute	*WindowsRedditWallpaperUpdater.SystemTrayIcon.exe*

If you want to automatically run the application at startup, you can use [Task Scheduler](https://msdn.microsoft.com/en-us/library/windows/desktop/aa383614(v=vs.85).aspx).


Note: Obviously I should make this more user friendly, like at least zip the files, but I'm going to let you do some work too for now :)

### Next

This application runs in the background as an icon in your system tray.

You can skip the current wallpaper by right-clicking the icon selecting Next Wallpaper.

### Choose your favourite subreddit

Open WindowsRedditWallpaperUpdaterSystemTrayIcon.exe.config in a text editor and update the **value** of rssUrl:    
```xml
<add key="rssUrl" value="https://www.reddit.com/r/EarthPorn/.rss"/>
```

### Change the refresh interval speed

Open WindowsRedditWallpaperUpdaterSystemTrayIcon.exe.config in a text editor and update the **value** of refreshIntervalInMinutes:

```xml
<add key="refreshIntervalInMinutes" value="60"/>
```

### Behind a proxy

Open WindowsRedditWallpaperUpdaterSystemTrayIcon.exe.config in a text editor and add the following section **after startup**:

```xml
<system.net>
 <defaultProxy useDfeaultCredentials="true">
  <proxy scriptLocation="http://your-proxy-script-location/here.dat" />
  </defaultProxy>
 </system.net>
```
