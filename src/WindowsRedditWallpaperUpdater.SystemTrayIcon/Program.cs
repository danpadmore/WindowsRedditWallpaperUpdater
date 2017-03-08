using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsRedditWallpaperUpdater.SystemTrayIcon
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;
            Application.Run(new ApplicationConfig());
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            EventLog.WriteEntry("Application", $"{typeof(Program).FullName} failed: {e.Exception}", EventLogEntryType.Error);
        }
    }
}