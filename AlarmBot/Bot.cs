using System.Diagnostics;

namespace AlarmBot
{
	public static class Bot
	{
        static private readonly int A_DAY_MILLISECONDS = 86400000;
        static private readonly int HOURS_TO_CHECK_NEW_PRODUCTS = 21;
        static private readonly int MINUTES_TO_CHECK_NEW_PRODUCTS = 00;
        static private readonly int HOURS_TO_CHECK_TODAY_DRAW = 07;
        static private readonly int MINUTES_TO_CHECK_TODAY_DRAW = 00;

        static private readonly System.Timers.Timer NEW_PRODUCT_TIMER = new System.Timers.Timer();
        static private readonly System.Timers.Timer TODAY_DRAW_TIMER = new System.Timers.Timer();

        public static bool IsRunning { get; private set; } = false;

        public static bool On()
        {
            Debug.Assert(BrandManager.IsSetBrands);
            Debug.Assert(MessengerManager.IsSetMessengers);

            if (IsRunning)
            {
                Debug.Assert(false, "Bot is Already running");
                return false;
            }

            // Set Timer
            NEW_PRODUCT_TIMER.Interval = A_DAY_MILLISECONDS;
            NEW_PRODUCT_TIMER.Elapsed += async (sender, e) => await checkNewProducts();
            NEW_PRODUCT_TIMER.AutoReset = true;

            TODAY_DRAW_TIMER.Interval = A_DAY_MILLISECONDS;
            TODAY_DRAW_TIMER.Elapsed += (sender, e) => setTodayDrawNotification();
            TODAY_DRAW_TIMER.AutoReset = true;

            startAllTimers();

            IsRunning = true;
            return true;
        }

        public static void Off()
        {
            NEW_PRODUCT_TIMER.Enabled = false;
            TODAY_DRAW_TIMER.Enabled = false;
            IsRunning = false;
        }

        private static void startAllTimers()
        {
            System.Timers.Timer ignitionNewProduct = new System.Timers.Timer();
            System.Timers.Timer ignitionTodayDraw = new System.Timers.Timer();

            DateTime newProductsTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, HOURS_TO_CHECK_NEW_PRODUCTS, MINUTES_TO_CHECK_NEW_PRODUCTS, 0);
            DateTime todayDrawTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, HOURS_TO_CHECK_TODAY_DRAW, MINUTES_TO_CHECK_TODAY_DRAW, 0);

            if (DateTime.Now > newProductsTime)
            {
                newProductsTime = newProductsTime.AddDays(1);
            }

            if (DateTime.Now > todayDrawTime)
            {
                todayDrawTime = todayDrawTime.AddDays(1);
            }

            ignitionNewProduct.Interval = (newProductsTime - DateTime.Now).TotalMilliseconds;
            ignitionNewProduct.Elapsed += (sender, e) => { NEW_PRODUCT_TIMER.Start(); };
            ignitionNewProduct.Elapsed += async (sender, e) => { await checkNewProducts(); };
            ignitionNewProduct.Start();

            ignitionTodayDraw.Interval = (todayDrawTime - DateTime.Now).TotalMilliseconds;
            ignitionTodayDraw.Elapsed += (sender, e) => { TODAY_DRAW_TIMER.Start(); };
            ignitionTodayDraw.Elapsed += (sender, e) => { setTodayDrawNotification(); };
            ignitionTodayDraw.Start();
        }

        private static async Task checkNewProducts()
        {
            List<ProductInfo> newProducts = await BrandManager.CheckNewProducts();

            if (newProducts.Count > 0)
            {
                DB.InsertProducts(newProducts);
            }
        }

        private static void setTodayDrawNotification()
        {
            List<ProductInfo> todayDrawProduct = DB.GetTodayDrawProducts(); // Change draw products from DB to BrandManager

            if (todayDrawProduct.Count == 0)
            {
                return;
            }

            MessengerManager.SetNotification(todayDrawProduct);

            foreach (ProductInfo p in todayDrawProduct)
            {
                BrandManager.RemoveProduct(p);
            }

            DB.DeleteProducts(todayDrawProduct);
        }
    }
}
