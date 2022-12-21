using HtmlAgilityPack;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AlarmBot
{
    public sealed class Nike : Brand
    {
        private readonly HttpClient client;
        public readonly string BaseUrl = "http://www.nike.com";

        public Nike(EBrand brand, string url)
            : base(brand, url)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", "Chrome/108.0.5359.124");
        }

        public override async Task CheckNewProduct()
        {
            var response = await client.GetAsync("http://www.nike.com/kr/launch?s=upcoming");
            string content = await response.Content.ReadAsStringAsync();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            var list = doc.DocumentNode.SelectNodes("//a[@class='card-link d-sm-b']");

        }

        public override void LoadProductsFromDatabase()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                const string query = "SELECT product_url, draw_date FROM draw_info";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, "draw_alarm");

                DataTable table = dataSet.Tables[0];

                for (int i = 0; i < table.Rows.Count; ++i)
                {
                    var row = table.Rows[i];
                    DateTime date = new DateTime(((DateTime)row["draw_date"]).Year, ((DateTime)row["draw_date"]).Month, ((DateTime)row["draw_date"]).Day);

                    ProductInfo product = new ProductInfo(BrandName, (string)row["product_url"], date);

                    upcommingProducts.Add(product);

                    Console.WriteLine(product.BrandName);
                    Console.WriteLine(product.Url);
                    Console.WriteLine(product.Date);
                }
            }
        }
    }
}
