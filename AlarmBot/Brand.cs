using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using MySql.Data.MySqlClient;

namespace AlarmBot
{
    public abstract class Brand
    {
        public readonly EBrand BrandName;
        public readonly string Url;

        public Brand(EBrand brandName, string url)
        {
            BrandName = brandName;
            Url = url;
        }

        public abstract Task<List<ProductInfo>> GetNewProduct();

        public List<ProductInfo> GetProductsFromDatabase()
        {
            List<ProductInfo> products = new List<ProductInfo>(64);

            DataSet data = DB.GetProducts(BrandName);
            DataTable table = data.Tables[0];

            for (int i = 0; i < table.Rows.Count; ++i)
            {
                var row = table.Rows[i];
                DateTime date = new DateTime(((DateTime)row["draw_start_time"]).Year, ((DateTime)row["draw_start_time"]).Month, ((DateTime)row["draw_start_time"]).Day, ((DateTime)row["draw_start_time"]).Hour, ((DateTime)row["draw_start_time"]).Minute, ((DateTime)row["draw_start_time"]).Second);

                ProductInfo product = new ProductInfo((uint)row["id"], BrandName, (string)row["type_name"], (string)row["product_name"], (uint)row["price"], (string)row["url"], (uint)row["url_hash"], date, (string)row["img_url"]);

                products.Add(product);
            }

            return products;
        }

        protected abstract ProductInfo makeProductInfoByHTML(HtmlDocument itemDoc, uint urlHash);

        protected uint urlFNVHash(string url)
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
