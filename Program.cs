using AlarmBot.AlarmBot;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Asn1.Sec;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using ZstdSharp.Unsafe;

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
        public static readonly int DATA_COUNT = 256;
        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            PerformanceTest.StartTest();
        }

        private static void InsertData()
        {
            ProductInfo[] products = new ProductInfo[DATA_COUNT];

            int TODAY_DATA_COUNT = (int)(DATA_COUNT * 0.05 + 0.5);
            for (int i = 0; i < TODAY_DATA_COUNT; ++i)
            {
                int randomIndex = random.Next(DATA_COUNT);

                while (products[randomIndex] != null)
                {
                    randomIndex = random.Next(DATA_COUNT);
                }

                products[randomIndex] = new ProductInfo(
                    EBrand.Nike,
                    $"test_type{i}",
                    $"test_name{i}",
                    random.Next(10000, 1000001),
                    $"url{i}",
                    DateOnly.FromDateTime(DateTime.Now),
                    DateTime.Now,
                    $"img_url{i}"
                );
            }

            for (int i = 0; i < DATA_COUNT; ++i)
            {
                if (products[i] != null)
                {
                    continue;
                }

                int randomAddDateNum = random.Next(1, 16);
                DateTime date = DateTime.Now.AddDays(randomAddDateNum);
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