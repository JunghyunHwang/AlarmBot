using System;
using System.Diagnostics;
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
        private static readonly System.Timers.Timer NewProductTimer = new System.Timers.Timer();
        private static readonly System.Timers.Timer TodayDrawTimer = new System.Timers.Timer();
        private static readonly List<Bot> bots = new List<Bot>(16);

        public static int CheckNewDrawHours = 21;
        public static int CheckTodayDrawHours = 7;
        private static bool IsRunning = false;

        public static void StartTimer()
        {
            if (IsRunning)
            {
                Debug.Assert(false, "Already running draw timer");
                return;
            }

            setTimerElapsedEventHandler();
            setBots();

            startCheckNewProductTimer();
            startCheckTodayDrawTimer();

            IsRunning = true;
        }

        private static void setTimerElapsedEventHandler()
        {
            Debug.Assert(!IsRunning);

            NewProductTimer.Elapsed += async (sender, e) => await checkNewProducts();
            TodayDrawTimer.Elapsed += (sender, e) => setTodayNotification();
        }

        private static void setBots()
        {
            Debug.Assert(!IsRunning);

            bots.Add(new TelegramBot());
        }

        private static void startCheckNewProductTimer()
        {
            DateTime checkNewProductsTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, CheckNewDrawHours, 0, 0, 0);

            if (DateTime.Now > checkNewProductsTime)
            {
                checkNewProductsTime = checkNewProductsTime.AddDays(1);
            }

            NewProductTimer.Interval = (double)(checkNewProductsTime - DateTime.Now).TotalMilliseconds;

            NewProductTimer.Start();
        }

        private static async Task checkNewProducts()
        {
            NewProductTimer.Stop();

            var newProducts = await BrandManager.CheckNewProducts();

            if (newProducts.Count > 0)
            {
                DB.InsertProducts(newProducts);
            }

            startCheckNewProductTimer();
        }

        private static void startCheckTodayDrawTimer()
        {
            DateTime checkTodayDrawTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, CheckTodayDrawHours, 0, 0, 0);

            if (DateTime.Now > checkTodayDrawTime)
            {
                checkTodayDrawTime = checkTodayDrawTime.AddDays(1);
            }

            TodayDrawTimer.Interval = (double)(checkTodayDrawTime - DateTime.Now).TotalMilliseconds;

            TodayDrawTimer.Start();
        }

        private static void setTodayNotification()
        {
            NewProductTimer.Stop();

            List<ProductInfo> todayDrawProducts = DB.GetTodayDrawProducts();

            foreach (var b in bots)
            {
                b.SetNotification(todayDrawProducts);
            }

            startCheckTodayDrawTimer();
        }
    }
}
