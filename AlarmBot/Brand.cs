using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace AlarmBot
{
    public abstract class Brand
    {
        public readonly EBrand BrandName;
        protected readonly string URL;
        protected PriorityQueue<ProductInfo, DateTime> products = new PriorityQueue<ProductInfo, DateTime>(64);

        public Brand(EBrand brandName, string url)
        {
            BrandName = brandName;
            URL = url;
        }

        public abstract Task<List<ProductInfo>> GetNewProduct();

        public abstract void RemoveTodayDrawProducts();

        public void SetProducts(List<ProductInfo> lp)
        {
            foreach (ProductInfo p in lp)
            {
                products.Enqueue(p, p.StartTime);
            }
        }

        protected abstract ProductInfo makeProductInfoByHTML(HtmlDocument itemDoc, string url);
    }
}
