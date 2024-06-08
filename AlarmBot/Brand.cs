using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace AlarmBot
{
    public abstract class Brand
    {
        public readonly EBrand BrandName;
        protected readonly string URL;
        protected Dictionary<string, ProductInfo> products = new Dictionary<string, ProductInfo>(64);

        public Brand(EBrand brandName, string url)
        {
            BrandName = brandName;
            URL = url;
        }

        public abstract Task<List<ProductInfo>> GetNewProduct();

        public abstract void RemoveProduct(ProductInfo product);

        public void LoadProductByDB()
        {
            DB.GetProductsByBrandName(BrandName, products);
        }

        protected abstract ProductInfo makeProductInfoByHTML(HtmlDocument itemDoc, string url);
    }
}
