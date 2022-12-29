using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AlarmBot
{
    public static class DrawTimer
    {
        private readonly static System.Timers.Timer CheckDrawTimer = new System.Timers.Timer();
        private readonly static int CheckNewDrawHours = 21;

        public static void SetTimerElapsedEventHandler()
        {
            CheckDrawTimer.Elapsed += async (sender, e) => await callCheckNewDraw();
        }

        public static void StartCheckDrawTimer()
        {
            DateTime nowTime = DateTime.Now;
            DateTime checkDrawTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, CheckNewDrawHours, 0, 0, 0);

            if (nowTime > checkDrawTime)
            {
                checkDrawTime = checkDrawTime.AddDays(1);
            }

            double tickTime = (double)(checkDrawTime - DateTime.Now).TotalMilliseconds;

            CheckDrawTimer.Interval = tickTime;
            CheckDrawTimer.Start();
        }

        private static async Task callCheckNewDraw()
        {
            CheckDrawTimer.Stop();

            await BrandManager.CheckNewProduct();

            StartCheckDrawTimer();
        }
    }
}
