using AlarmBot.Test;
using System.Diagnostics;

namespace AlarmBot
{
    internal static class PerformanceTest
    {
        private static readonly int TEST_COUNT = 10000;
        private static readonly Random RANDOM = new Random();

        public static void StartTestGetTodayDrawProducts()
        {
#if DEBUG
            Console.WriteLine("====    Debug build    ====");
#else
            Console.WriteLine("====   Release build   ====");
#endif
            Console.WriteLine($"====  Data count: {TestDataGenerator.GeneratedDataCount}  ====");
            Console.WriteLine($"==== Test count: {TEST_COUNT} ====");

            TestDataStructure.SetData();

            TestGetTodayDrawProductsFromDB();
            TestGetTodayDrawProductsFromPQ();
            TestGetTodayDrawProductsFromList();
            TestGetTodayDrawProductsFromArray();
            TestGetTodayDrawProductsFromDictionary();
        }

        public static void StartTestProductExists()
        {
#if DEBUG
            Console.WriteLine("====    Debug build    ====");
#else
            Console.WriteLine("====   Release build   ====");
#endif
            Console.WriteLine($"====  Data count: {TestDataGenerator.GeneratedDataCount}  ====");
            Console.WriteLine($"==== Test count: {TEST_COUNT} ====");

            TestDataStructure.SetData();

            TestProductExistsFromPQ();
            TestProductExistsFromList();
            TestProductExistsFromArray();
            TestProductExistsFromDictionary();
        }

        public static void TestGetTodayDrawProductsFromDB()
        {
            long nanoPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency; // 1틱에 몇 나노초
            long numTicks = 0;
            long maxTicks = 0;
            long minTicks = Int64.MaxValue;

            for (int i = 0; i < TEST_COUNT; ++i)
            {
                Stopwatch timeGetFromDB = Stopwatch.StartNew();

                var products = DB.GetTodayDrawProducts();

                timeGetFromDB.Stop();

                if (i == 0)
                {
                    continue;
                }

                if (maxTicks < timeGetFromDB.ElapsedTicks)
                {
                    maxTicks = timeGetFromDB.ElapsedTicks;
                }
                if (minTicks > timeGetFromDB.ElapsedTicks)
                {
                    minTicks = timeGetFromDB.ElapsedTicks;
                }
                numTicks += timeGetFromDB.ElapsedTicks;
            }

            Console.WriteLine("Get today draw products from DB");
            Console.WriteLine($"  Slowest: {maxTicks} tick");
            Console.WriteLine($"  Fastest: {minTicks} tick");
            Console.WriteLine($"  Average: {numTicks / (TEST_COUNT - 1)} tick, {numTicks * nanoPerTick / (TEST_COUNT - 1)} nanoseconds");
            Console.WriteLine();
        }

        public static void TestGetTodayDrawProductsFromPQ()
        {
            Debug.Assert(TestDataStructure.IsSetData, "Data not set");

            long nanoPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            long numTicks = 0;
            long maxTicks = 0;
            long minTicks = Int64.MaxValue;

            for (int i = 0; i < TEST_COUNT; ++i)
            {
                Stopwatch timeGetFromQueue = Stopwatch.StartNew();

                var products = TestDataStructure.GetTodayDrawProductsFromPQ();

                timeGetFromQueue.Stop();

                if (i == 0)
                {
                    continue;
                }

                if (maxTicks < timeGetFromQueue.ElapsedTicks)
                {
                    maxTicks = timeGetFromQueue.ElapsedTicks;
                }
                if (minTicks > timeGetFromQueue.ElapsedTicks)
                {
                    minTicks = timeGetFromQueue.ElapsedTicks;
                }
                numTicks += timeGetFromQueue.ElapsedTicks;
            }

            Console.WriteLine("Get today draw products from PQ");
            Console.WriteLine($"  Slowest: {maxTicks} tick");
            Console.WriteLine($"  Fastest: {minTicks} tick");
            Console.WriteLine($"  Average: {numTicks / (TEST_COUNT - 1)} tick, {numTicks * nanoPerTick / (TEST_COUNT - 1)} nanoseconds");
            Console.WriteLine();
        }

        public static void TestGetTodayDrawProductsFromList()
        {
            Debug.Assert(TestDataStructure.IsSetData, "Data not set");

            long nanoPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            long numTicks = 0;
            long maxTicks = 0;
            long minTicks = Int64.MaxValue;

            for (int i = 0; i < TEST_COUNT; ++i)
            {
                Stopwatch timeGetFromQueue = Stopwatch.StartNew();

                var products = TestDataStructure.GetTodayDrawProductsFromList();

                timeGetFromQueue.Stop();

                if (i == 0)
                {
                    continue;
                }

                if (maxTicks < timeGetFromQueue.ElapsedTicks)
                {
                    maxTicks = timeGetFromQueue.ElapsedTicks;
                }
                if (minTicks > timeGetFromQueue.ElapsedTicks)
                {
                    minTicks = timeGetFromQueue.ElapsedTicks;
                }
                numTicks += timeGetFromQueue.ElapsedTicks;
            }

            Console.WriteLine("Get today draw products from List");
            Console.WriteLine($"  Slowest: {maxTicks} tick");
            Console.WriteLine($"  Fastest: {minTicks} tick");
            Console.WriteLine($"  Average: {numTicks / (TEST_COUNT - 1)} tick, {numTicks * nanoPerTick / (TEST_COUNT - 1)} nanoseconds");
            Console.WriteLine();
        }

        public static void TestGetTodayDrawProductsFromArray()
        {
            Debug.Assert(TestDataStructure.IsSetData, "Data not set");

            long nanoPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            long numTicks = 0;
            long maxTicks = 0;
            long minTicks = Int64.MaxValue;

            for (int i = 0; i < TEST_COUNT; ++i)
            {
                Stopwatch timeGetFromQueue = Stopwatch.StartNew();

                var products = TestDataStructure.GetTodayDrawProductsFromArray();

                timeGetFromQueue.Stop();

                if (i == 0)
                {
                    continue;
                }

                if (maxTicks < timeGetFromQueue.ElapsedTicks)
                {
                    maxTicks = timeGetFromQueue.ElapsedTicks;
                }
                if (minTicks > timeGetFromQueue.ElapsedTicks)
                {
                    minTicks = timeGetFromQueue.ElapsedTicks;
                }
                numTicks += timeGetFromQueue.ElapsedTicks;
            }

            Console.WriteLine("Get today draw products from Array");
            Console.WriteLine($"  Slowest: {maxTicks} tick");
            Console.WriteLine($"  Fastest: {minTicks} tick");
            Console.WriteLine($"  Average: {numTicks / (TEST_COUNT - 1)} tick, {numTicks * nanoPerTick / (TEST_COUNT - 1)} nanoseconds");
            Console.WriteLine();
        }

        public static void TestGetTodayDrawProductsFromDictionary()
        {
            Debug.Assert(TestDataStructure.IsSetData, "Data not set");

            long nanoPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            long numTicks = 0;
            long maxTicks = 0;
            long minTicks = Int64.MaxValue;

            for (int i = 0; i < TEST_COUNT; ++i)
            {
                Stopwatch timeGetFromQueue = Stopwatch.StartNew();

                var products = TestDataStructure.GetTodayDrawProductsFromDictionary();

                timeGetFromQueue.Stop();

                if (i == 0)
                {
                    continue;
                }

                if (maxTicks < timeGetFromQueue.ElapsedTicks)
                {
                    maxTicks = timeGetFromQueue.ElapsedTicks;
                }
                if (minTicks > timeGetFromQueue.ElapsedTicks)
                {
                    minTicks = timeGetFromQueue.ElapsedTicks;
                }
                numTicks += timeGetFromQueue.ElapsedTicks;
            }

            Console.WriteLine("Get today draw products from Dictionary");
            Console.WriteLine($"  Slowest: {maxTicks} tick");
            Console.WriteLine($"  Fastest: {minTicks} tick");
            Console.WriteLine($"  Average: {numTicks / (TEST_COUNT - 1)} tick, {numTicks * nanoPerTick / (TEST_COUNT - 1)} nanoseconds");
            Console.WriteLine();
        }

        public static void TestProductExistsFromPQ()
        {
            Debug.Assert(TestDataStructure.IsSetData, "Data not set");

            long nanoPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            long numTicks = 0;
            long maxTicks = 0;
            long minTicks = Int64.MaxValue;

            ProductInfo nonExistentProduct = new ProductInfo(EBrand.Nike, "type_name-1", "name-1", -1, "url-1", DateOnly.FromDateTime(DateTime.Now), DateTime.Now, "img-1");

            int NON_EXISTENT_PRODUCT_TEST_COUNT = TEST_COUNT / 2;

            for (int i = 0; i < NON_EXISTENT_PRODUCT_TEST_COUNT; ++i)
            {
                Stopwatch timeGetFromQueue = Stopwatch.StartNew();

                bool bHasProduct = TestDataStructure.HasProductsByPQ(nonExistentProduct);

                timeGetFromQueue.Stop();

                Debug.Assert(!bHasProduct);

                if (i == 0)
                {
                    continue;
                }

                if (maxTicks < timeGetFromQueue.ElapsedTicks)
                {
                    maxTicks = timeGetFromQueue.ElapsedTicks;
                }
                if (minTicks > timeGetFromQueue.ElapsedTicks)
                {
                    minTicks = timeGetFromQueue.ElapsedTicks;
                }
                numTicks += timeGetFromQueue.ElapsedTicks;
            }

            List<ProductInfo> allData = TestDataStructure.GetAllData();
            int randomIndex = RANDOM.Next(allData.Count);

            for (int i = NON_EXISTENT_PRODUCT_TEST_COUNT; i < TEST_COUNT; ++i)
            {
                Stopwatch timeGetFromQueue = Stopwatch.StartNew();

                bool bHasProduct = TestDataStructure.HasProductsByPQ(allData[randomIndex]);

                timeGetFromQueue.Stop();

                Debug.Assert(bHasProduct);

                if (i == 0)
                {
                    continue;
                }

                if (maxTicks < timeGetFromQueue.ElapsedTicks)
                {
                    maxTicks = timeGetFromQueue.ElapsedTicks;
                }
                if (minTicks > timeGetFromQueue.ElapsedTicks)
                {
                    minTicks = timeGetFromQueue.ElapsedTicks;
                }
                numTicks += timeGetFromQueue.ElapsedTicks;
            }

            Console.WriteLine("Product Exists from PQ");
            Console.WriteLine($"  Slowest: {maxTicks} tick");
            Console.WriteLine($"  Fastest: {minTicks} tick");
            Console.WriteLine($"  Average: {numTicks / (TEST_COUNT - 1)} tick, {numTicks * nanoPerTick / (TEST_COUNT - 1)} nanoseconds");
            Console.WriteLine();
        }

        public static void TestProductExistsFromList()
        {
            Debug.Assert(TestDataStructure.IsSetData, "Data not set");

            long nanoPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            long numTicks = 0;
            long maxTicks = 0;
            long minTicks = Int64.MaxValue;

            ProductInfo nonExistentProduct = new ProductInfo(EBrand.Nike, "type_name-1", "name-1", -1, "url-1", DateOnly.FromDateTime(DateTime.Now), DateTime.Now, "img-1");

            int NON_EXISTENT_PRODUCT_TEST_COUNT = TEST_COUNT / 2;

            for (int i = 0; i < NON_EXISTENT_PRODUCT_TEST_COUNT; ++i)
            {
                Stopwatch timeGetFromQueue = Stopwatch.StartNew();

                bool bHasProduct = TestDataStructure.HasProductsByList(nonExistentProduct);

                timeGetFromQueue.Stop();

                Debug.Assert(!bHasProduct);

                if (i == 0)
                {
                    continue;
                }

                if (maxTicks < timeGetFromQueue.ElapsedTicks)
                {
                    maxTicks = timeGetFromQueue.ElapsedTicks;
                }
                if (minTicks > timeGetFromQueue.ElapsedTicks)
                {
                    minTicks = timeGetFromQueue.ElapsedTicks;
                }
                numTicks += timeGetFromQueue.ElapsedTicks;
            }

            List<ProductInfo> allData = TestDataStructure.GetAllData();
            int randomIndex = RANDOM.Next(allData.Count);

            for (int i = NON_EXISTENT_PRODUCT_TEST_COUNT; i < TEST_COUNT; ++i)
            {
                Stopwatch timeGetFromQueue = Stopwatch.StartNew();

                bool bHasProduct = TestDataStructure.HasProductsByList(allData[randomIndex]);

                timeGetFromQueue.Stop();

                Debug.Assert(bHasProduct);

                if (i == 0)
                {
                    continue;
                }

                if (maxTicks < timeGetFromQueue.ElapsedTicks)
                {
                    maxTicks = timeGetFromQueue.ElapsedTicks;
                }
                if (minTicks > timeGetFromQueue.ElapsedTicks)
                {
                    minTicks = timeGetFromQueue.ElapsedTicks;
                }
                numTicks += timeGetFromQueue.ElapsedTicks;
            }

            Console.WriteLine("Product Exists from List");
            Console.WriteLine($"  Slowest: {maxTicks} tick");
            Console.WriteLine($"  Fastest: {minTicks} tick");
            Console.WriteLine($"  Average: {numTicks / (TEST_COUNT - 1)} tick, {numTicks * nanoPerTick / (TEST_COUNT - 1)} nanoseconds");
            Console.WriteLine();
        }

        public static void TestProductExistsFromArray()
        {
            Debug.Assert(TestDataStructure.IsSetData, "Data not set");

            long nanoPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            long numTicks = 0;
            long maxTicks = 0;
            long minTicks = Int64.MaxValue;

            ProductInfo nonExistentProduct = new ProductInfo(EBrand.Nike, "type_name-1", "name-1", -1, "url-1", DateOnly.FromDateTime(DateTime.Now), DateTime.Now, "img-1");

            int NON_EXISTENT_PRODUCT_TEST_COUNT = TEST_COUNT / 2;

            for (int i = 0; i < NON_EXISTENT_PRODUCT_TEST_COUNT; ++i)
            {
                Stopwatch timeGetFromQueue = Stopwatch.StartNew();

                bool bHasProduct = TestDataStructure.HasProductsByArray(nonExistentProduct);

                timeGetFromQueue.Stop();

                Debug.Assert(!bHasProduct);

                if (i == 0)
                {
                    continue;
                }

                if (maxTicks < timeGetFromQueue.ElapsedTicks)
                {
                    maxTicks = timeGetFromQueue.ElapsedTicks;
                }
                if (minTicks > timeGetFromQueue.ElapsedTicks)
                {
                    minTicks = timeGetFromQueue.ElapsedTicks;
                }
                numTicks += timeGetFromQueue.ElapsedTicks;
            }

            List<ProductInfo> allData = TestDataStructure.GetAllData();
            int randomIndex = RANDOM.Next(allData.Count);

            for (int i = NON_EXISTENT_PRODUCT_TEST_COUNT; i < TEST_COUNT; ++i)
            {
                Stopwatch timeGetFromQueue = Stopwatch.StartNew();

                bool bHasProduct = TestDataStructure.HasProductsByArray(allData[randomIndex]);

                timeGetFromQueue.Stop();

                Debug.Assert(bHasProduct);

                if (i == 0)
                {
                    continue;
                }

                if (maxTicks < timeGetFromQueue.ElapsedTicks)
                {
                    maxTicks = timeGetFromQueue.ElapsedTicks;
                }
                if (minTicks > timeGetFromQueue.ElapsedTicks)
                {
                    minTicks = timeGetFromQueue.ElapsedTicks;
                }
                numTicks += timeGetFromQueue.ElapsedTicks;
            }

            Console.WriteLine("Product Exists from Array");
            Console.WriteLine($"  Slowest: {maxTicks} tick");
            Console.WriteLine($"  Fastest: {minTicks} tick");
            Console.WriteLine($"  Average: {numTicks / (TEST_COUNT - 1)} tick, {numTicks * nanoPerTick / (TEST_COUNT - 1)} nanoseconds");
            Console.WriteLine();
        }

        public static void TestProductExistsFromDictionary()
        {
            Debug.Assert(TestDataStructure.IsSetData, "Data not set");

            long nanoPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            long numTicks = 0;
            long maxTicks = 0;
            long minTicks = Int64.MaxValue;

            ProductInfo nonExistentProduct = new ProductInfo(EBrand.Nike, "type_name-1", "name-1", -1, "url-1", DateOnly.FromDateTime(DateTime.Now), DateTime.Now, "img-1");

            int NON_EXISTENT_PRODUCT_TEST_COUNT = TEST_COUNT / 2;

            for (int i = 0; i < NON_EXISTENT_PRODUCT_TEST_COUNT; ++i)
            {
                Stopwatch timeGetFromQueue = Stopwatch.StartNew();

                bool bHasProduct = TestDataStructure.HasProductsByDictionary(nonExistentProduct);

                timeGetFromQueue.Stop();

                Debug.Assert(!bHasProduct);

                if (i == 0)
                {
                    continue;
                }

                if (maxTicks < timeGetFromQueue.ElapsedTicks)
                {
                    maxTicks = timeGetFromQueue.ElapsedTicks;
                }
                if (minTicks > timeGetFromQueue.ElapsedTicks)
                {
                    minTicks = timeGetFromQueue.ElapsedTicks;
                }
                numTicks += timeGetFromQueue.ElapsedTicks;
            }

            List<ProductInfo> allData = TestDataStructure.GetAllData();
            int randomIndex = RANDOM.Next(allData.Count);

            for (int i = NON_EXISTENT_PRODUCT_TEST_COUNT; i < TEST_COUNT; ++i)
            {
                Stopwatch timeGetFromQueue = Stopwatch.StartNew();

                bool bHasProduct = TestDataStructure.HasProductsByDictionary(allData[randomIndex]);

                timeGetFromQueue.Stop();

                Debug.Assert(bHasProduct);

                if (i == 0)
                {
                    continue;
                }

                if (maxTicks < timeGetFromQueue.ElapsedTicks)
                {
                    maxTicks = timeGetFromQueue.ElapsedTicks;
                }
                if (minTicks > timeGetFromQueue.ElapsedTicks)
                {
                    minTicks = timeGetFromQueue.ElapsedTicks;
                }
                numTicks += timeGetFromQueue.ElapsedTicks;
            }

            Console.WriteLine("Product Exists from Dictionary");
            Console.WriteLine($"  Slowest: {maxTicks} tick");
            Console.WriteLine($"  Fastest: {minTicks} tick");
            Console.WriteLine($"  Average: {numTicks / (TEST_COUNT - 1)} tick, {numTicks * nanoPerTick / (TEST_COUNT - 1)} nanoseconds");
            Console.WriteLine();
        }
    }
}
