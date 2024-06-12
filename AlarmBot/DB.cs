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

        static DB()
        {
            CONNECTION.Open();
        }

        public static List<ProductInfo> GetProductsByBrandName(EBrand brandName)
        {
            List<ProductInfo> products = new List<ProductInfo>(64);
            DataSet dataSet = new DataSet();

            string query = $"SELECT * FROM products WHERE brand_name='{brandName}'";

            MySqlDataAdapter adapter = new MySqlDataAdapter(query, CONNECTION);
            adapter.Fill(dataSet, "alarmbot");

            DataTable table = dataSet.Tables[0];

            for (int i = 0; i < table.Rows.Count; ++i)
            {
                DataRow row = table.Rows[i];
                DateTime drawDateTime = new DateTime(((DateTime)row["draw_start_time"]).Year, ((DateTime)row["draw_start_time"]).Month, ((DateTime)row["draw_start_time"]).Day, ((DateTime)row["draw_start_time"]).Hour, ((DateTime)row["draw_start_time"]).Minute, ((DateTime)row["draw_start_time"]).Second);
                DateOnly drawDate = DateOnly.FromDateTime(drawDateTime);

                products.Add(new ProductInfo(brandName, (string)row["type_name"], (string)row["product_name"], (int)row["price"], (string)row["url"], drawDate, drawDateTime, (string)row["img_url"]));
            }

            return products;
        }

        public static List<ProductInfo> GetTodayDrawProducts()
        {
            List<ProductInfo> products = new List<ProductInfo>(64);
            DataSet dataSet = new DataSet();
            string query = $"SELECT * FROM products WHERE draw_date='{DateTime.Now.ToString("yyyy-MM-dd")}'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, CONNECTION);

            adapter.Fill(dataSet, "alarmbot");

            DataTable table = dataSet.Tables[0];

            for (int i = 0; i < table.Rows.Count; ++i)
            {
                var row = table.Rows[i];
                DateTime drawDateTime = new DateTime(((DateTime)row["draw_start_time"]).Year, ((DateTime)row["draw_start_time"]).Month, ((DateTime)row["draw_start_time"]).Day, ((DateTime)row["draw_start_time"]).Hour, ((DateTime)row["draw_start_time"]).Minute, ((DateTime)row["draw_start_time"]).Second);
                DateOnly drawDate = DateOnly.FromDateTime(drawDateTime);
                EBrand brandName;

                if (!Enum.TryParse((string)row["brand_name"], out brandName))
                {
                    Debug.Assert(false, "Wrong brand name");
                }

                products.Add(new ProductInfo(brandName, (string)row["type_name"], (string)row["product_name"], (int)row["price"], (string)row["url"], drawDate, drawDateTime, (string)row["img_url"]));
            }

            return products;
        }

        public static List<User> GetUsersByMessenger(EMessenger messenger)
        {
            List<User> users = new List<User>(128);
            DataSet dataSet = new DataSet();
            string query = $"SELECT chat_id FROM users WHERE messenger='{messenger}'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, CONNECTION);

            adapter.Fill(dataSet, "alarmbot");

            DataTable table = dataSet.Tables[0];

            for (int i = 0; i < table.Rows.Count; ++i)
            {
                var row = table.Rows[i];

                users.Add(new User(messenger, (string)row["chat_id"]));
            }

            return users;
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
                queryBuilder.Append($"VALUES ('{p.BrandName}', '{p.TypeName}', '{p.ProductName}', '{p.Price}', '{p.Url}', '{p.DrawDate}', '{p.StartTime.ToString("yyyy-MM-dd hh:mm:ss")}', '{p.ImgUrl}');");

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
