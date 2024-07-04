using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AlarmBot
{
    public static class MessageManager
    {
        private static readonly int HOURS_TO_CHECK_TODAY_DRAW = 07;
        private static readonly int MINUTES_TO_CHECK_TODAY_DRAW = 00;
        private static readonly System.Timers.Timer TODAY_DRAW_TIMER = new System.Timers.Timer();
        private static readonly Dictionary<DateTime, List<ProductInfo>> GROUP_BY_START_TIME = new Dictionary<DateTime, List<ProductInfo>>(16);

        public static bool IsRunning { get; private set; } = false;

        public static bool On()
        {
            if (IsRunning)
            {
                Debug.Assert(false, "Bot is Already running");
                return false;
            }

            TODAY_DRAW_TIMER.Interval = Program.A_DAY_MILLISECONDS;
            TODAY_DRAW_TIMER.Elapsed += (sender, e) => setTodayDrawNotification();
            TODAY_DRAW_TIMER.AutoReset = true;

            igniteTodayDrawTimer();

            IsRunning = true;
            return true;
        }

        private static void igniteTodayDrawTimer()
        {
            System.Timers.Timer ignitionTodayDraw = new System.Timers.Timer();
            DateTime todayDrawTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, HOURS_TO_CHECK_TODAY_DRAW, MINUTES_TO_CHECK_TODAY_DRAW, 0);

            if (DateTime.Now > todayDrawTime)
            {
                todayDrawTime = todayDrawTime.AddDays(1);
            }

            ignitionTodayDraw.Interval = (todayDrawTime - DateTime.Now).TotalMilliseconds;
            ignitionTodayDraw.Elapsed += (sender, e) => { TODAY_DRAW_TIMER.Start(); };
            ignitionTodayDraw.Elapsed += (sender, e) => { setTodayDrawNotification(); };
            ignitionTodayDraw.AutoReset = false;
            ignitionTodayDraw.Start();
        }

        private static void setTodayDrawNotification()
        {
            Debug.Assert(GROUP_BY_START_TIME.Count == 0, "Has an Unexpired timers");

            List<ProductInfo> todayDrawProduct = BrandManager.GetTodayDrawProducts();
            
            foreach (ProductInfo p in todayDrawProduct)
            {
                if (!GROUP_BY_START_TIME.ContainsKey(p.StartTime))
                {
                    GROUP_BY_START_TIME.Add(p.StartTime, new List<ProductInfo>(todayDrawProduct.Count));

                    System.Timers.Timer drawTimer = new System.Timers.Timer();
                    drawTimer.Interval = (p.StartTime - DateTime.Now).TotalMilliseconds;
                    drawTimer.Elapsed += (sender, e) => sendMessage(drawTimer, p.StartTime);
                    drawTimer.AutoReset = false;
                    drawTimer.Start();
                }

                GROUP_BY_START_TIME[p.StartTime].Add(p);
            }
        }

        private static void sendMessage(System.Timers.Timer drawTimer, DateTime startTime)
        {
            foreach (ProductInfo p in GROUP_BY_START_TIME[startTime])
            {
                p.SendMessage();
            }
            
            GROUP_BY_START_TIME.Remove(startTime);
            drawTimer.Dispose();
        }
    }
}
