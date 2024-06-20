using AlarmBot.Test;
using System.Diagnostics;

namespace AlarmBot
{
    public static class TestDataStructure
    {
        public static bool IsSetData {  get; private set; } = false;

        private static readonly List<ProductInfo> allData;

        private static PriorityQueue<ProductInfo, DateTime> productsPQ = new PriorityQueue<ProductInfo, DateTime>(TestDataGenerator.GeneratedDataCount);
        private static List<ProductInfo> productsList = new List<ProductInfo>(TestDataGenerator.GeneratedDataCount);
        private static ProductInfo[] productsArray = new ProductInfo[TestDataGenerator.GeneratedDataCount];
        private static Dictionary<string, ProductInfo> productsMap = new Dictionary<string, ProductInfo>(256);
        public static ref readonly List<ProductInfo> refAllData => ref allData;
        static TestDataStructure()
        {
            allData = DB.GetAllProducts();
        }

        public static void SetData()
        {
            if (IsSetData)
            {
                return;
            }

            int i = 0;

            foreach (ProductInfo p in allData)
            {
                productsPQ.Enqueue(p, p.StartTime);
                productsList.Add(p);
                productsArray[i++] = p;
                productsMap.Add(p.Url, p);

                Debug.Assert(i <= TestDataGenerator.GeneratedDataCount);
            }

            IsSetData = true;
        }

        public static List<ProductInfo> GetAllData()
        {
            return refAllData;
        }

        public static List<ProductInfo> GetTodayDrawProductsFromPQ()
        {
            List<ProductInfo> result = new List<ProductInfo>(TestDataGenerator.GeneratedDataCount);
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
            List<ProductInfo> result = new List<ProductInfo>(TestDataGenerator.GeneratedDataCount);
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
            List<ProductInfo> result = new List<ProductInfo>(TestDataGenerator.GeneratedDataCount);
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

        public static List<ProductInfo> GetTodayDrawProductsFromDictionary()
        {
            List<ProductInfo> result = new List<ProductInfo>(TestDataGenerator.GeneratedDataCount);
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            foreach (KeyValuePair<string, ProductInfo> pair in productsMap)
            {
                if (pair.Value.DrawDate == today)
                {
                    result.Add(pair.Value);
                }
            }

            return result;
        }
        
        public static bool HasProductsByPQ(ProductInfo product)
        {
            List<ProductInfo> temp = new List<ProductInfo>(productsPQ.Count);
            bool result = false;

            while (productsPQ.Count > 0)
            {
                if (productsPQ.Peek().Equals(product))
                {
                    result = true;
                    break;
                }

                temp.Add(productsPQ.Dequeue());
            }

            foreach (ProductInfo p in temp)
            {
                productsPQ.Enqueue(p, p.StartTime);
            }

            return result;
        }

        public static bool HasProductsByList(ProductInfo product)
        {
            foreach (ProductInfo p in productsList)
            {
                if (p.Equals(product))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasProductsByArray(ProductInfo product)
        {
            foreach (ProductInfo p in productsArray)
            {
                if (p.Equals(product))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasProductsByDictionary(ProductInfo product)
        {
            return productsMap.ContainsKey(product.Url);
        }
    }
}
