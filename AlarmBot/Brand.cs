using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace AlarmBot
{
    public abstract class Brand
    {
        public readonly EBrand BrandName;
        public readonly string Url;
        protected List<ProductInfo> upcommingProducts;
        protected static string connectionString = ConfigurationManager.ConnectionStrings["AlarmBot"].ConnectionString;

        public Brand(EBrand brandName, string url)
        {
            BrandName = brandName;
            Url = url;
            upcommingProducts = new List<ProductInfo>(32);
        }

        public abstract Task CheckNewProduct();

        public abstract void LoadProductsFromDatabase();
    }
}
