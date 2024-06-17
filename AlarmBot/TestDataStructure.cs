using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmBot.AlarmBot
{
    public static class TestDataStructure
    {
        public static bool IsSetData {  get; private set; } = false;
        private static PriorityQueue<ProductInfo, DateTime> productsPQ = new PriorityQueue<ProductInfo, DateTime>(Program.DATA_COUNT);
        private static List<ProductInfo> productsList = new List<ProductInfo>(Program.DATA_COUNT);
        private static ProductInfo[] productsArray = new ProductInfo[Program.DATA_COUNT];

        public static void SetData()
        {
            List<ProductInfo> list = DB.GetProductsByBrandName(EBrand.Nike);

            int i = 0;

            foreach (var p in list)
            {
                productsPQ.Enqueue(p, p.StartTime);
                productsList.Add(p);
                productsArray[i++] = p;

                Debug.Assert(i <= Program.DATA_COUNT);
            }

            IsSetData = true;
        }

        public static List<ProductInfo> GetTodayDrawProductsFromPQ()
        {
            List<ProductInfo> result = new List<ProductInfo>(Program.DATA_COUNT);
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            while (productsPQ.Peek().DrawDate == today)
            {
                result.Add(productsPQ.Dequeue());
            }

            foreach (ProductInfo p in result)
            {
                productsPQ.Enqueue(p, p.StartTime);
            }

            return result;
        }

        public static List<ProductInfo> GetTodayDrawProductsFromList()
        {
            List<ProductInfo> result = new List<ProductInfo>(Program.DATA_COUNT);
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            foreach (ProductInfo p in productsList)
            {
                if (p.DrawDate == today)
                {
                    result.Add(p);
                }
            }

            return result;
        }

        public static List<ProductInfo> GetTodayDrawProductsFromArray()
        {
            List<ProductInfo> result = new List<ProductInfo>(Program.DATA_COUNT);
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            foreach (ProductInfo p in productsArray)
            {
                if (p.DrawDate == today)
                {
                    result.Add(p);
                }
            }

            return result;
        }
    }
}
