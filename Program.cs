using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MySqlX.XDevAPI;

namespace AlarmBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //await TestLoadDataFromDatabase();

            TestRegex();

            Console.WriteLine("No prob");
        }

        private static async Task TestLoadDataFromDatabase()
        {
            Brand nike = new Nike(EBrand.Nike, "www.nike.com");

            BrandManager.AddBrand(nike);
            await BrandManager.GetNewProduct();
        }

        private static void TestRegex()
        {
            string price = "239,000";
            string date = "12. 27. 오전 10:00출시";

            Regex rxPrice = new Regex("\\d+");
            Regex rxDateTime = new Regex("(\\d{1,2})");

            MatchCollection tempPrice = rxPrice.Matches(price);
            MatchCollection tempDateTime = rxDateTime.Matches(date);

            string strPrice = tempPrice[0].Value + tempPrice[1].Value;
            int month = int.Parse(tempDateTime[0].Value);
            int day = int.Parse(tempDateTime[1].Value);
            int hours = int.Parse(tempDateTime[2].Value);
            int minutes = int.Parse(tempDateTime[3].Value);

            DateTime dateTime = new DateTime(DateTime.Now.Year, month, day, hours, minutes, 0);

            uint p = uint.Parse(strPrice);

            Console.WriteLine(p);
        }
    }
}