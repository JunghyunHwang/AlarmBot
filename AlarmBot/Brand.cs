using HtmlAgilityPack;

namespace AlarmBot
{
    public abstract class Brand
    {
        public readonly EBrand BrandName;
        protected readonly string URL;
        protected Dictionary<string, ProductInfo> upcomingProducts = new Dictionary<string, ProductInfo>(256);

        public Brand(EBrand brandName, string url)
        {
            BrandName = brandName;
            URL = url;
        }

        public abstract Task<List<ProductInfo>> GetNewProduct();

        public List<ProductInfo> GetTodayDrawProducts()
        {
            List<ProductInfo> result = new List<ProductInfo>(16);
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            foreach (KeyValuePair<string, ProductInfo> pair in upcomingProducts)
            {
                if (pair.Value.DrawDate == today)
                {
                    result.Add(pair.Value);
                }
            }

            return result;
        }

        public void RemoveTodayDrawProducts()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            foreach (ProductInfo p in upcomingProducts.Values)
            {
                if (p.DrawDate == today)
                {
                    upcomingProducts.Remove(p.Url);
                }
            }
        }

        public void AddProducts(List<ProductInfo> products)
        {
            foreach (ProductInfo p in products)
            {
                upcomingProducts.Add(p.Url, p);
            }
        }

        protected abstract ProductInfo makeProductInfoByHTML(HtmlDocument itemDoc, string url);
    }
}
