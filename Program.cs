using System.Data;
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
            TestDB();

            Console.WriteLine("No prob");
        }

        private static async Task TestLoadDataFromDatabase()
        {
            Brand nike = new Nike(EBrand.Nike, "www.nike.com");

            BrandManager.AddBrand(nike);
            await BrandManager.GetNewProduct();
        }

        private static void TestDB()
        {
            List<ProductInfo> products = new List<ProductInfo>(2);
            DateTime date = DateTime.Now;

            ProductInfo p1 = new ProductInfo(EBrand.Nike, "에어 조던1 로우", "조던 X 트래비스 스캇", 1600000, "https://www.naver.com", 123, date, "https://cdn-images.farfetch-contents.com/17/03/72/29/17037229_35056975_1000.jpg");
            ProductInfo p2 = new ProductInfo(EBrand.Nike, "SB 덩크", "트래비스 스캇", 2500000, "https://www.youtube.com", 1, date, "https://cdn-images.farfetch-contents.com/17/03/72/29/17037229_35056975_1000.jpg");

            products.Add(p1);
            products.Add(p2);

            DB.InsertProducts(products);

            Nike n = new Nike(EBrand.Nike, "https://www.nike.com/kr/launch?s=upcoming");
            var pros = n.GetProductsFromDatabase();

            Debug.Assert(pros.Count == 2);
            Debug.Assert(pros[0].Equals(p1));
            Debug.Assert(pros[1].Equals(p2));

            DB.DeleteProducts(products);

            pros = n.GetProductsFromDatabase();
            Debug.Assert(pros.Count == 0);
        }
    }
}