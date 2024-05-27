using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace AlarmBot
{
    public abstract class Brand
    {
        public readonly EBrand BrandName;
        protected readonly string URL;
        protected List<ProductInfo> products = new List<ProductInfo>(64);

        public Brand(EBrand brandName, string url)
        {
            BrandName = brandName;
            URL = url;
        }

        public abstract Task<List<ProductInfo>> GetNewProduct();

        public abstract void RemoveProduct(ProductInfo product);

        public void LoadProductByDB()
        {
            products.AddRange(DB.GetProductsByBrandName(BrandName));
        }

        protected abstract ProductInfo makeProductInfoByHTML(HtmlDocument itemDoc, string url, uint urlHash);

        protected static uint urlFNVHash(string url)
        {
            const uint FNV_PRIME_32 = 16777619;
            uint hash = 2166136261U;

            for (int i = 0; i < url.Length; ++i)
            {
                hash *= FNV_PRIME_32;
                hash ^= url[i];
            }

            return hash;
        }
    }
}
