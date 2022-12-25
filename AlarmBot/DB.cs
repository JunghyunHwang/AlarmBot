using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmBot
{
    public static class DB
    {
        public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["AlarmBot"].ConnectionString;

        public static DataSet GetProducts(EBrand brand)
        {
            DataSet dataSet = new DataSet();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string query = $"SELECT * FROM draw_info WHERE brand_name={brand}";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                adapter.Fill(dataSet, "draw_alarm");
            }

            return dataSet;
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
                    queryBuilder.Append("INSERT INTO draw_info (brand_name, type_name, product_name, price, url, url_hash, draw_start_time, img_url)");
                    queryBuilder.Append($"VALUES ({p.BrandName}, {p.TypeName}, {p.ProductName}, {p.Price}, {p.Url}, {p.UrlHash}, {p.StartTime}, {p.ImgUrl});");

                    insertCommand.CommandText = queryBuilder.ToString();
                    insertCommand.ExecuteNonQuery();
                }
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
                    insertCommand.CommandText = $"DELETE FROM draw_info WHERE id={p.ID}";
                    insertCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
