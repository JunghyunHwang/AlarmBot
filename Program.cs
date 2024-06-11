using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Asn1.Sec;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace AlarmBot
{
    /*
     * TODO
     * Add IDisposable: DB.cs
     * Remove product: Bot.cs
     * Add method that find today draw products: Bot.cs
     * Fix products Dictionary to PriorityQueue: Brands.cs
     */
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
        }

        private static async Task testScraping()
        {
            Brand brand = new Nike(EBrand.Nike, "https://www.nike.com/kr/launch?s=upcoming");
            await brand.GetNewProduct();
        }
    }
}