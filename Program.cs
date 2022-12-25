using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.VisualBasic;
using MySqlX.XDevAPI;

namespace AlarmBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //await TestBrandManager();

            Console.WriteLine("No prob");
        }

        private static async Task TestBrandManager()
        {
            Brand nike = new Nike(EBrand.Nike, "www.nike.com");

            BrandManager.AddBrand(nike);
            await BrandManager.GetNewProduct();
        }
    }
}