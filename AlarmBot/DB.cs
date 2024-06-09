using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Text;

/* TODO
  * Remove urlHash  
*/

namespace AlarmBot
{
    public static class DB
    {
        static public readonly string ConnectionString = ConfigurationManager.ConnectionStrings["AlarmBot"].ConnectionString;

        public static void GetProductsByBrandName(EBrand brandName, Dictionary<string, ProductInfo> outProducts)
        {
            DataSet dataSet = new DataSet();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string query = $"SELECT * FROM draw_info WHERE brand_name='{brandName}'";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                adapter.Fill(dataSet, "draw_alarm");

                DataTable table = dataSet.Tables[0];

                for (int i = 0; i < table.Rows.Count; ++i)
                {
                    DataRow row = table.Rows[i];
                    DateTime drawDateTime = new DateTime(((DateTime)row["draw_start_time"]).Year, ((DateTime)row["draw_start_time"]).Month, ((DateTime)row["draw_start_time"]).Day, ((DateTime)row["draw_start_time"]).Hour, ((DateTime)row["draw_start_time"]).Minute, ((DateTime)row["draw_start_time"]).Second);
                    DateOnly drawDate = DateOnly.FromDateTime(drawDateTime);

                    outProducts.Add((string)row["url"], new ProductInfo(brandName, (string)row["type_name"], (string)row["product_name"], (uint)row["price"], (string)row["url"], drawDate, drawDateTime, (string)row["img_url"]));
                }
            }
        }

        public static List<ProductInfo> GetTodayDrawProducts()
        {
            List<ProductInfo> products = new List<ProductInfo>(64);
            DataSet dataSet = new DataSet();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string query = $"SELECT * FROM draw_info WHERE draw_date='{DateTime.Now.ToString("yyyy-MM-dd")}'";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                adapter.Fill(dataSet, "draw_alarm");

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
                    
                    products.Add(new ProductInfo(brandName, (string)row["type_name"], (string)row["product_name"], (uint)row["price"], (string)row["url"], drawDate, drawDateTime, (string)row["img_url"]));
                }
            }

            return products;
        }

        public static List<User> GetUsersByMessenger(EMessenger messenger)
        {

            List<User> users = new List<User>(128);
            DataSet dataSet = new DataSet();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string query = $"SELECT chat_id FROM users WHERE messenger='{messenger}'";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                adapter.Fill(dataSet, "draw_alarm");

                DataTable table = dataSet.Tables[0];

                for (int i = 0; i < table.Rows.Count; ++i)
                {
                    var row = table.Rows[i];

                    users.Add(new User(messenger, (string)row["chat_id"]));
                }
            }

            return users;
        }

        public static void InsertProducts(List<ProductInfo> products)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                StringBuilder queryBuilder = new StringBuilder(256);
                MySqlCommand insertCommand = new MySqlCommand();
                insertCommand.Connection = connection;

                foreach (var p in products)
                {
                    queryBuilder.Clear();
                    queryBuilder.Append("INSERT INTO draw_info (brand_name, type_name, product_name, price, url, draw_date, draw_start_time, img_url) ");
                    queryBuilder.Append($"VALUES ('{p.BrandName}', '{p.TypeName}', '{p.ProductName}', '{p.Price}', '{p.Url}', '{p.DrawDate}', '{p.StartTime.ToString("yyyy-MM-dd hh:mm:ss")}', '{p.ImgUrl}');");

                    insertCommand.CommandText = queryBuilder.ToString();
                    insertCommand.ExecuteNonQuery();
                }
            }
        }
        public static void DeleteProduct(ProductInfo product)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                MySqlCommand insertCommand = new MySqlCommand();
                insertCommand.Connection = connection;

                insertCommand.CommandText = $"DELETE FROM draw_info WHERE url='{product.Url}'";
                insertCommand.ExecuteNonQuery();
            }
        }

        public static void DeleteProducts(List<ProductInfo> products)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                MySqlCommand insertCommand = new MySqlCommand();
                insertCommand.Connection = connection;

                foreach (var p in products)
                {
                    insertCommand.CommandText = $"DELETE FROM draw_info WHERE url='{p.Url}'";
                    insertCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
