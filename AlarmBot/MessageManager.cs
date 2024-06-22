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

        private static readonly List<System.Timers.Timer> DRAW_TIMERS = new List<System.Timers.Timer>();

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
            Debug.Assert(DRAW_TIMERS.Count == 0, "Has an Unexpired timers");
            List<ProductInfo> todayDrawProduct = BrandManager.GetTodayDrawProducts();

            foreach (ProductInfo p in todayDrawProduct)
            {
                Debug.Assert(DateTime.Now < p.StartTime, "Out of time product...");
                System.Timers.Timer drawTimer = new System.Timers.Timer();

                drawTimer.Interval = (p.StartTime - DateTime.Now).TotalMilliseconds;
                drawTimer.Elapsed += (sender, e) => sendMessage(drawTimer, p);
                drawTimer.AutoReset = false;
                drawTimer.Start();

                DRAW_TIMERS.Add(drawTimer);
            }
        }

        private static void sendMessage(System.Timers.Timer timer, ProductInfo product)
        {
            product.SendMessage();

            DRAW_TIMERS.Remove(timer);
            timer.Dispose();
        }
    }
}
