using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MySqlX.XDevAPI;

namespace AlarmBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            TestLoadDataFromDatabase();
            await Test();

            Console.WriteLine("No prob");
        }

        private static void TestLoadDataFromDatabase()
        {
            Brand nike = new Nike(EBrand.Nike, "www.nike.com");

            BrandManager.AddBrand(nike);
            BrandManager.CheckNewProduct();
        }

        private static async Task Test()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://www.nike.com");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", "Chrome/108.0.5359.124");

            var response = await client.GetAsync("http://www.nike.com/kr/launch?s=upcoming");
            string content = await response.Content.ReadAsStringAsync();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            var list = doc.DocumentNode.SelectNodes("//a[@class='card-link d-sm-b']"); // product-card ncss-row mr0-sm ml0-sm

            Console.WriteLine(list[0].Attributes[3].Value);
        }
    }
}