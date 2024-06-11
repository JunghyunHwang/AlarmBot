using System.Diagnostics;

namespace AlarmBot
{
    public static class Bot
	{
        private static readonly int A_DAY_MILLISECONDS = 86400000;
        private static readonly int HOURS_TO_CHECK_NEW_PRODUCTS = 21;
        private static readonly int MINUTES_TO_CHECK_NEW_PRODUCTS = 00;
        private static readonly int HOURS_TO_CHECK_TODAY_DRAW = 07;
        private static readonly int MINUTES_TO_CHECK_TODAY_DRAW = 00;

        private static readonly System.Timers.Timer NEW_PRODUCT_TIMER = new System.Timers.Timer();
        private static readonly System.Timers.Timer TODAY_DRAW_TIMER = new System.Timers.Timer();

        public static bool IsRunning { get; private set; } = false;

        private static readonly Dictionary<EBrand, Brand> BRANDS = new Dictionary<EBrand, Brand>((int)EBrand.Count);

        public static bool On()
        {
            Debug.Assert(MessengerManager.IsSetMessengers, "MessengerManager is not set up");
            if (IsRunning)
            {
                Debug.Assert(false, "Bot is Already running");
                return false;
            }

            BRANDS.Add(EBrand.Nike, new Nike(EBrand.Nike, "https://www.nike.com/kr/launch?s=upcoming"));
            Debug.Assert(BRANDS.Count == (int)EBrand.Count);

            foreach (Brand b in BRANDS.Values)
            {
                b.LoadProductByDB();
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
            Debug.Assert(IsRunning, "Bot is not running");
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
            ignitionNewProduct.AutoReset = false;
            ignitionNewProduct.Start();

            ignitionTodayDraw.Interval = (todayDrawTime - DateTime.Now).TotalMilliseconds;
            ignitionTodayDraw.Elapsed += (sender, e) => { TODAY_DRAW_TIMER.Start(); };
            ignitionTodayDraw.Elapsed += (sender, e) => { setTodayDrawNotification(); };
            ignitionTodayDraw.AutoReset = false;
            ignitionTodayDraw.Start();
        }

        private static async Task checkNewProducts()
        {
            List<ProductInfo> newProducts = new List<ProductInfo>(64);

            foreach (Brand b in BRANDS.Values)
            {
                newProducts.AddRange(await b.GetNewProduct());
            }

            if (newProducts.Count > 0)
            {
                DB.InsertProducts(newProducts);
            }
        }

        private static void setTodayDrawNotification()
        {
            List<ProductInfo> todayDrawProduct = DB.GetTodayDrawProducts(); // Change draw products from DB to BrandManager

            if (todayDrawProduct.Count > 0)
            {
                MessengerManager.SetNotification(todayDrawProduct);

                foreach (ProductInfo p in todayDrawProduct)
                {
                    BRANDS[p.BrandName].RemoveProduct(p);
                }

                DB.DeleteProducts(todayDrawProduct);
            }
        }
    }
}
