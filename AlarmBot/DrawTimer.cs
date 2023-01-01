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
        private static readonly System.Timers.Timer newProductTimer = new System.Timers.Timer();
        private static readonly System.Timers.Timer todayDrawTimer = new System.Timers.Timer();
        private static readonly List<Bot> bots = new List<Bot>(16);
        private static readonly int checkNewDrawHours = 21;
        private static readonly int checkTodayDrawHours = 1;
        private static bool IsRunning = false;

        public static int StartTimer()
        {
            if (!BrandManager.IsSetBrand)
            {
                Debug.Assert(false, "BrandManager does not setting!");
                return -1;
            }
            else if (IsRunning)
            {
                Debug.Assert(false, "Already running draw timer!");
                return -1;
            }

            setTimerElapsedEventHandler();
            setMessengerBot();

            startCheckNewProductTimer();
            startCheckTodayDrawTimer();

            IsRunning = true;

            return 1;
        }

        private static void setTimerElapsedEventHandler()
        {
            Debug.Assert(!IsRunning);

            newProductTimer.Elapsed += async (sender, e) => await checkNewProducts();
            todayDrawTimer.Elapsed += (sender, e) => setTodayNotification();
        }

        private static void setMessengerBot()
        {
            Debug.Assert(!IsRunning);
            
            bots.Add(new TelegramBot());
        }

        private static void startCheckNewProductTimer()
        {                                          
            DateTime checkNewProductsTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, checkNewDrawHours, 0, 0);

            if (DateTime.Now > checkNewProductsTime)
            {
                checkNewProductsTime = checkNewProductsTime.AddDays(1);
            }

            newProductTimer.Interval = (checkNewProductsTime - DateTime.Now).TotalMilliseconds;

            newProductTimer.Start();
        }

        private static void startCheckTodayDrawTimer()
        {
            DateTime checkTodayDrawTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, checkTodayDrawHours, 38, 0);

            if (DateTime.Now > checkTodayDrawTime)
            {
                checkTodayDrawTime = checkTodayDrawTime.AddDays(1);
            }

            todayDrawTimer.Interval = (checkTodayDrawTime - DateTime.Now).TotalMilliseconds;

            todayDrawTimer.Start();
        }

        private static async Task checkNewProducts()
        {
            newProductTimer.Stop();

            var newProducts = await BrandManager.CheckNewProducts();

            if (newProducts.Count > 0)
            {
                DB.InsertProducts(newProducts);
            }

            startCheckNewProductTimer();
        }

        private static void setTodayNotification()
        {
            newProductTimer.Stop();

            List<ProductInfo> todayDrawProducts = DB.GetTodayDrawProducts();

            foreach (var b in bots)
            {
                b.SetNotification(todayDrawProducts);
            }

            startCheckTodayDrawTimer();
        }
    }
}
