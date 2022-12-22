using HtmlAgilityPack;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
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
            StringBuilder urlBuilder = new StringBuilder(128);

            var response = await client.GetAsync("http://www.nike.com/kr/launch?s=upcoming");
            string content = await response.Content.ReadAsStringAsync();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            var list = doc.DocumentNode.SelectNodes("//a[@class=\"card-link d-sm-b\"]");

            Debug.Assert(list.Count % 2 == 0);

            for (int i = 0; i < list.Count / 2; ++i)
            {
                int linkIndex = list[i].Attributes.Count - 1;

                var res = await client.GetAsync(list[i].Attributes[linkIndex].Value);
                string itemHTML = await res.Content.ReadAsStringAsync();

                HtmlDocument itemDoc = new HtmlDocument();
                itemDoc.LoadHtml(itemHTML);

                var br = itemDoc.DocumentNode.SelectNodes("//br");

                if (br.Count == 4)
                {
                    urlBuilder.Append(BaseUrl).Append(list[i].Attributes[linkIndex].Value);

                    var tagTypeName = itemDoc.DocumentNode.SelectSingleNode("//h1[@class=\"headline-5 pb3-sm\"]");
                    var tagSneakersName = itemDoc.DocumentNode.SelectSingleNode("//h5[@class=\"headline-1 pb3-sm\"]");
                    var tagPrice = itemDoc.DocumentNode.SelectSingleNode("//div[@class=\"headline-5 pb6-sm fs14-sm fs16-md\"]");
                    var tagDate = itemDoc.DocumentNode.SelectSingleNode("//div[@class=\"available-date-component\"]");

                    Regex rxPrice = new Regex("\\d+");
                    Regex rxDate = new Regex("(\\d{1,2})\\/(\\d{1,2})");
                    Regex rxTime = new Regex("(\\d{1,2}:\\d{1,2})");


                    MatchCollection price = rxPrice.Matches(tagPrice.InnerText);
                    MatchCollection date = rxDate.Matches(tagDate.InnerText);
                    MatchCollection time = rxTime.Matches(tagDate.InnerText);
                }
            }
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
