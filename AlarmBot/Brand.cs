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

        public abstract Task<List<ProductInfo>> GetNewProduct(List<ProductInfo> existingProducts);

        protected abstract ProductInfo makeProductInfoByHTML(HtmlDocument itemDoc, string url, uint urlHash);

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
