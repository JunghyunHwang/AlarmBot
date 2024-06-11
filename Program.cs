using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Asn1.Sec;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace AlarmBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            /*
            Bot.On();
            bool bIsExit = true;

            while (bIsExit)
            {
                if (Console.ReadLine() == "exit")
                {
                    bIsExit = false;
                }
            }
            */

            await testScraping();
            testDBInsert();
        }

        private static async Task testScraping()
        {
            Brand brand = new Nike(EBrand.Nike, "https://www.nike.com/kr/launch?s=upcoming");
            await brand.GetNewProduct();
        }

        private static void testDBInsert()
        {
            List<ProductInfo> list = new List<ProductInfo>
            {
                new ProductInfo(EBrand.Nike, "조던", "녹타", 1, "www.hello.com", DateOnly.FromDateTime(DateTime.Now), DateTime.Now, "이미지다~")
            };

            DB.InsertProducts(list);
        }
    }
}