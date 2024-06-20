using AlarmBot.AlarmBot;

namespace AlarmBot
{
    internal class Program
    {
        public static readonly int DATA_COUNT = 256;
        public static readonly float todayDrawProductsRatio = 0.2f;

        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            //InsertData();

            PerformanceTest.StartTestGetTodayDrawProducts();
        }

        private static void InsertData()
        {
            ProductInfo[] products = new ProductInfo[DATA_COUNT];

            int TODAY_DATA_COUNT = (int)(DATA_COUNT * todayDrawProductsRatio + 0.5);
            for (int i = 0; i < TODAY_DATA_COUNT; ++i)
            {
                int randomIndex = random.Next(DATA_COUNT);

                while (products[randomIndex] != null)
                {
                    randomIndex = random.Next(DATA_COUNT);
                }

                products[randomIndex] = new ProductInfo(
                    EBrand.Nike,
                    $"test_type{randomIndex}",
                    $"test_name{randomIndex}",
                    random.Next(10000, 1000001),
                    $"url{randomIndex}",
                    DateOnly.FromDateTime(DateTime.Now),
                    DateTime.Now,
                    $"img_url{randomIndex}"
                );
            }

            for (int i = 0; i < DATA_COUNT; ++i)
            {
                if (products[i] != null)
                {
                    continue;
                }

                DateTime date = DateTime.Now.AddDays(random.Next(1, 16));
                products[i] = new ProductInfo(
                    EBrand.Nike,
                    $"test_type{i}",
                    $"test_name{i}",
                    random.Next(10000, 1000001),
                    $"url{i}",
                    DateOnly.FromDateTime(date),
                    date,
                    $"img_url{i}"
                );
            }

            DB.InsertProducts(products.ToList());
        }
    }
}