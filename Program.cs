using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Asn1.Sec;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace AlarmBot
{
    /*
     * TODO
     * Add IDisposable: DB.cs
     * Remove product: Bot.cs
     * Add method that find today draw products: Bot.cs
     * Fix products Dictionary to PriorityQueue: Brands.cs
     */
    internal class Program
    {
        private static readonly Random random = new Random();
        static void Main(string[] args)
        {
            DateOnly d1 = DateOnly.FromDateTime(DateTime.Now);
            DateOnly d2 = DateOnly.FromDateTime(DateTime.Now);
            DateOnly d3 = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            DateOnly d4 = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));

            Debug.Assert(d1 == d2);
            Debug.Assert(d1 < d3);
            Debug.Assert(d1 > d4);
        }

        private static void GetSingleProductFromDB()
        {
            const int testCount = 10000;
            long nanoPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency; // 1틱에 몇 나노초
            long numTicks = 0;
            long maxTicks = 0;
            long minTicks = Int64.MaxValue;

            for (int i = 0; i < testCount; ++i)
            {
                Stopwatch timeGetFromDB = Stopwatch.StartNew();

                var products = DB.GetProductsByBrandName(EBrand.Nike);

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

            Console.WriteLine("Get single products from DB");
            Console.WriteLine($"  Slowest: {maxTicks} tick");
            Console.WriteLine($"  Fastest: {minTicks} tick");
            Console.WriteLine($"  Average: {numTicks / testCount} tick, {numTicks * nanoPerTick / testCount} nanoseconds");
            Console.WriteLine();
        }

        private static void GetMultiProductsByBrandNameFromDB()
        {
            const int testCount = 10000;
            long nanoPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency; // 1틱에 몇 나노초
            long numTicks = 0;
            long maxTicks = 0;
            long minTicks = Int64.MaxValue;
            
            for (int i = 0; i < testCount; ++i)
            {
                Stopwatch timeGetFromDB = Stopwatch.StartNew();

                var products = DB.GetProductsByBrandName(EBrand.Nike);

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

            Console.WriteLine("Get multi products by brand name from DB");
            Console.WriteLine($"  Slowest: {maxTicks} tick");
            Console.WriteLine($"  Fastest: {minTicks} tick");
            Console.WriteLine($"  Average: {numTicks / testCount} tick, {numTicks * nanoPerTick / testCount} nanoseconds");
            Console.WriteLine();
        }

        private static void GetTodayDrawProductsFromDB()
        {
            const int testCount = 10000;
            long nanoPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency; // 1틱에 몇 나노초
            long numTicks = 0;
            long maxTicks = 0;
            long minTicks = Int64.MaxValue;

            for (int i = 0; i < testCount; ++i)
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
            Console.WriteLine($"  Average: {numTicks / testCount} tick, {numTicks * nanoPerTick / testCount} nanoseconds");
            Console.WriteLine();
        }

        private static void InsertData()
        {
            const int DATA_COUNT = 128;
            List<ProductInfo> products = new List<ProductInfo>(DATA_COUNT);

            for (int i = 0; i < DATA_COUNT; ++i)
            {
                DateTime dateTime = new DateTime(2024, 6, random.Next(12, 16), 10, 0, 0);
                DateOnly onlyDate = DateOnly.FromDateTime(dateTime);

                products.Add(
                    new ProductInfo(
                        EBrand.Nike,
                        "test_type",
                        "test_name",
                        random.Next(10000, 1000001),
                        $"url{i}",
                        onlyDate,
                        dateTime,
                        $"img_url{i}"
                        ));
            }

            DB.InsertProducts(products);
        }
    }
}