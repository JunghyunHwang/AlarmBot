using System;

namespace AlarmBot.Test
{
    public static class TestDataGenerator
    {
        private static readonly Random RANDOM = new Random();
        public static int GeneratedDataCount { get; private set; } = 0;

        private static void TestDataGenerate(int dataCount, float todayDrawProductsRatio)
        {
            GeneratedDataCount = dataCount;

            ProductInfo[] products = new ProductInfo[dataCount];

            int TODAY_DATA_COUNT = (int)(dataCount * todayDrawProductsRatio + 0.5);
            for (int i = 0; i < TODAY_DATA_COUNT; ++i)
            {
                int randomIndex = RANDOM.Next(dataCount);

                while (products[randomIndex] != null)
                {
                    randomIndex = RANDOM.Next(dataCount);
                }

                products[randomIndex] = new ProductInfo(
                    EBrand.Nike,
                    $"test_type{randomIndex}",
                    $"test_name{randomIndex}",
                    RANDOM.Next(10000, 1000001),
                    $"url{randomIndex}",
                    DateOnly.FromDateTime(DateTime.Now),
                    DateTime.Now,
                    $"img_url{randomIndex}"
                );
            }

            for (int i = 0; i < dataCount; ++i)
            {
                if (products[i] != null)
                {
                    continue;
                }

                DateTime date = DateTime.Now.AddDays(RANDOM.Next(1, 16));
                products[i] = new ProductInfo(
                    EBrand.Nike,
                    $"test_type{i}",
                    $"test_name{i}",
                    RANDOM.Next(10000, 1000001),
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
