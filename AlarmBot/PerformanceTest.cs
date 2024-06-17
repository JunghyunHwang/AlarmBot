using System.Diagnostics;

namespace AlarmBot.AlarmBot
{
    internal static class PerformanceTest
    {
        private static readonly int TEST_COUNT = 10000;

        public static void StartTest()
        {
#if DEBUG
            Console.WriteLine("====   Debug build   ====");
#else
            Console.WriteLine("====  Release build  ====");
#endif
            Console.WriteLine($"==== Data count: {Program.DATA_COUNT} ====");
            TestDataStructure.SetData();

            GetTodayDrawProductsFromDB();
            GetTodayDrawProductsFromDataTable();

            GetTodayDrawProductsFromPQ();
            GetTodayDrawProductsFromList();
            GetTodayDrawProductsFromArray();
        }

        public static void GetTodayDrawProductsFromDB()
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
            Console.WriteLine();
        }

        public static void GetTodayDrawProductsFromDataTable()
        {
            long nanoPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            long numTicks = 0;
            long maxTicks = 0;
            long minTicks = Int64.MaxValue;

            for (int i = 0; i < TEST_COUNT; ++i)
            {
                Stopwatch timeGetFromQueue = Stopwatch.StartNew();

                var products = DB.GetTodayProductsFromTable();

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

            Console.WriteLine("Get today draw products from DataTable");
            Console.WriteLine($"  Slowest: {maxTicks} tick");
            Console.WriteLine($"  Fastest: {minTicks} tick");
            Console.WriteLine($"  Average: {numTicks / (TEST_COUNT - 1)} tick, {numTicks * nanoPerTick / (TEST_COUNT - 1)} nanoseconds");
            Console.WriteLine();
            Console.WriteLine();
        }

        public static void GetTodayDrawProductsFromPQ()
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
            Console.WriteLine();
        }

        public static void GetTodayDrawProductsFromList()
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
            Console.WriteLine();
        }

        public static void GetTodayDrawProductsFromArray()
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
            Console.WriteLine();
        }
    }
}
