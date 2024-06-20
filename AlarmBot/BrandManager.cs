using System.Diagnostics;

namespace AlarmBot
{
    public static class BrandManager
	{
        private static readonly int HOURS_TO_CHECK_NEW_PRODUCTS = 21;
        private static readonly int MINUTES_TO_CHECK_NEW_PRODUCTS = 00;
        private static readonly System.Timers.Timer NEW_PRODUCT_TIMER = new System.Timers.Timer();

        private static readonly Dictionary<EBrand, Brand> BRANDS = new Dictionary<EBrand, Brand>((int)EBrand.Count);

        public static bool IsRunning { get; private set; } = false;

        public static bool On()
        {
            if (IsRunning)
            {
                Debug.Assert(false, "Bot is Already running");
                return false;
            }

            BRANDS.Add(EBrand.Nike, new Nike(EBrand.Nike, "https://www.nike.com/kr/launch?s=upcoming"));
            Debug.Assert(BRANDS.Count == (int)EBrand.Count);

            foreach (Brand b in BRANDS.Values)
            {
                b.AddProducts(DB.GetProductsByBrandName(b.BrandName));
            }

            // Set Timer
            NEW_PRODUCT_TIMER.Interval = Program.A_DAY_MILLISECONDS;
            NEW_PRODUCT_TIMER.Elapsed += async (sender, e) => await checkNewProducts();
            NEW_PRODUCT_TIMER.AutoReset = true;

            startCheckNewProductsTimers();
            
            IsRunning = true;
            return true;
        }

        public static List<ProductInfo> GetTodayDrawProducts()
        {
            List<ProductInfo> todayDrawProduct = new List<ProductInfo>(Program.DEFAULT_LIST_COUNT);

            foreach (Brand b in BRANDS.Values)
            {
                todayDrawProduct.AddRange(b.GetTodayDrawProducts());
                b.RemoveTodayDrawProducts();
            }

            return todayDrawProduct;
        }

        private static void startCheckNewProductsTimers()
        {
            System.Timers.Timer ignitionNewProduct = new System.Timers.Timer();

            DateTime newProductsTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, HOURS_TO_CHECK_NEW_PRODUCTS, MINUTES_TO_CHECK_NEW_PRODUCTS, 0);

            if (DateTime.Now > newProductsTime)
            {
                newProductsTime = newProductsTime.AddDays(1);
            }

            ignitionNewProduct.Interval = (newProductsTime - DateTime.Now).TotalMilliseconds;
            ignitionNewProduct.Elapsed += (sender, e) => { NEW_PRODUCT_TIMER.Start(); };
            ignitionNewProduct.Elapsed += async (sender, e) => { await checkNewProducts(); };
            ignitionNewProduct.AutoReset = false;
            ignitionNewProduct.Start();
        }

        private static async Task checkNewProducts()
        {
            Debug.Assert(IsRunning);

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
    }
}
