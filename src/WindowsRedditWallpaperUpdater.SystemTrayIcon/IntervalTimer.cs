using System;
using System.Timers;

namespace WindowsRedditWallpaperUpdater.SystemTrayIcon
{
    internal class IntervalTimer
    {
        private readonly Timer timer;
        private readonly Action methodToExecute;

        public IntervalTimer(Action methodToExecute, int intervalInMinutes)
        {
            this.methodToExecute = methodToExecute;

            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(OnIntervalElapsed);
            timer.Interval = TimeSpan.FromMinutes(intervalInMinutes).TotalMilliseconds;
            timer.Start();
        }

        private void OnIntervalElapsed(object sender, EventArgs e)
        {
            methodToExecute.Invoke();
            ResetInterval();
        }

        public void ResetInterval()
        {
            timer.Stop();
            timer.Start();
        }
    }
}