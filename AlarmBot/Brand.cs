using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Org.BouncyCastle.Asn1.Cmp;

namespace AlarmBot
{
    public abstract class Brand
    {
        public readonly EBrand BrandName;
        protected readonly string URL;
        protected PriorityQueue<ProductInfo, DateTime> products = new PriorityQueue<ProductInfo, DateTime>(256);
        protected Dictionary<string, ProductInfo> searchProducts = new Dictionary<string, ProductInfo>(256);

        public Brand(EBrand brandName, string url)
        {
            BrandName = brandName;
            URL = url;
        }

        public abstract Task<List<ProductInfo>> GetNewProduct();

        public List<ProductInfo> GetTodayDrawProducts()
        {
            List<ProductInfo> result = new List<ProductInfo>(16);
            DateOnly now = DateOnly.FromDateTime(DateTime.Now);

            while (products.Peek().DrawDate == now)
            {
                result.Add(products.Dequeue());
            }

            // Is it really necessary to restore? => how about GetTodayDrawProductsAndDelete()?
            foreach (ProductInfo p in result)
            {
                products.Enqueue(p, p.StartTime);
            }

            return result;
        }

        public void RemoveTodayDrawProducts()
        {
            DateOnly now = DateOnly.FromDateTime(DateTime.Now);

            while (products.Peek().DrawDate <= now)
            {
                ProductInfo p = products.Dequeue();
                searchProducts.Remove(p.Url);
            }
        }

        public void AddProducts(List<ProductInfo> lp)
        {
            foreach (ProductInfo p in lp)
            {
                addProduct(p);
            }
        }

        protected abstract ProductInfo makeProductInfoByHTML(HtmlDocument itemDoc, string url);

        protected void addProduct(ProductInfo p)
        {
            products.Enqueue(p, p.StartTime);
            searchProducts.Add(p.Url, p);
        }
    }
}
