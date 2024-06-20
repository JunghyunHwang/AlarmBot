using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace AlarmBot
{
    public static class DB
    {
        private static readonly string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["AlarmBot"].ConnectionString;
        private static readonly MySqlConnection CONNECTION = new MySqlConnection(CONNECTION_STRING);
        private static readonly DataSet DATA_SET = new DataSet();

        public static int userCount { get; private set; } = 0;

        static DB()
        {
            CONNECTION.Open();
            // Todo:
            //  Get user count from DB and set userCount
            //  Add InsertUser() method
        }

        public static List<ProductInfo> GetAllProducts()
        {
            DATA_SET.Clear();
            List<ProductInfo> products = new List<ProductInfo>(Program.DEFAULT_LIST_COUNT);
            string query = $"SELECT * FROM products";
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, CONNECTION);

            adapter.Fill(DATA_SET, "alarmbot");

            DataTable table = DATA_SET.Tables[0];

            for (int i = 0; i < table.Rows.Count; ++i)
            {
                DataRow row = table.Rows[i];
                DateTime drawDateTime = (DateTime)row["draw_start_time"];
                DateOnly drawDate = DateOnly.FromDateTime(drawDateTime);

                products.Add(new ProductInfo((EBrand)row["brand_name"], (string)row["type_name"], (string)row["product_name"], (int)row["price"], (string)row["url"], drawDate, drawDateTime, (string)row["img_url"]));
            }

            return products;
        }

        public static List<ProductInfo> GetProductsByBrandName(EBrand brandName)
        {
            DATA_SET.Clear();
            List<ProductInfo> products = new List<ProductInfo>(Program.DEFAULT_LIST_COUNT);
            string query = $"SELECT * FROM products WHERE brand_name={(uint)brandName}";
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, CONNECTION);

            adapter.Fill(DATA_SET, "alarmbot");

            DataTable table = DATA_SET.Tables[0];

            for (int i = 0; i < table.Rows.Count; ++i)
            {
                DataRow row = table.Rows[i];
                DateTime drawDateTime = (DateTime)row["draw_start_time"];
                DateOnly drawDate = DateOnly.FromDateTime(drawDateTime);

                products.Add(new ProductInfo(brandName, (string)row["type_name"], (string)row["product_name"], (int)row["price"], (string)row["url"], drawDate, drawDateTime, (string)row["img_url"]));
            }

            return products;
        }
        
        public static List<ProductInfo> GetTodayDrawProducts()
        {
            DATA_SET.Clear();
            List<ProductInfo> products = new List<ProductInfo>(Program.DEFAULT_LIST_COUNT);
            string query = $"SELECT * FROM products WHERE draw_date='{DateTime.Now.ToString("yyyy-MM-dd")}'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, CONNECTION);

            adapter.Fill(DATA_SET, "alarmbot");

            DataTable table = DATA_SET.Tables[0];

            for (int i = 0; i < table.Rows.Count; ++i)
            {
                DataRow row = table.Rows[i];
                DateTime drawDateTime = new DateTime(((DateTime)row["draw_start_time"]).Year, ((DateTime)row["draw_start_time"]).Month, ((DateTime)row["draw_start_time"]).Day, ((DateTime)row["draw_start_time"]).Hour, ((DateTime)row["draw_start_time"]).Minute, ((DateTime)row["draw_start_time"]).Second);
                DateOnly drawDate = DateOnly.FromDateTime(drawDateTime);

                products.Add(new ProductInfo((EBrand)row["brand_name"], (string)row["type_name"], (string)row["product_name"], (int)row["price"], (string)row["url"], drawDate, drawDateTime, (string)row["img_url"]));
            }

            return products;
        }

        public static void InsertProducts(List<ProductInfo> products)
        {
            StringBuilder queryBuilder = new StringBuilder(256);
            MySqlCommand insertCommand = new MySqlCommand();
            insertCommand.Connection = CONNECTION;

            foreach (var p in products)
            {
                queryBuilder.Clear();
                queryBuilder.Append("INSERT INTO products (brand_name, type_name, product_name, price, url, draw_date, draw_start_time, img_url) ");
                queryBuilder.Append($"VALUES ('{(int)p.BrandName}', '{p.TypeName}', '{p.ProductName}', '{p.Price}', '{p.Url}', '{p.DrawDate}', '{p.StartTime.ToString("yyyy-MM-dd hh:mm:ss")}', '{p.ImgUrl}');");

                insertCommand.CommandText = queryBuilder.ToString();
                insertCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Product url is unique
        /// </summary>
        public static void DeleteProduct(ProductInfo product)
        {
            MySqlCommand insertCommand = new MySqlCommand();
            insertCommand.Connection = CONNECTION;

            insertCommand.CommandText = $"DELETE FROM products WHERE url='{product.Url}'";
            insertCommand.ExecuteNonQuery();
        }

        public static void DeleteProducts(List<ProductInfo> products)
        {
            MySqlCommand insertCommand = new MySqlCommand();
            insertCommand.Connection = CONNECTION;

            foreach (var p in products)
            {
                insertCommand.CommandText = $"DELETE FROM products WHERE url='{p.Url}'";
                insertCommand.ExecuteNonQuery();
            }
        }
    }
}
