using Org.BouncyCastle.Asn1.Sec;
using System.Data;
using System.Diagnostics;
using System.Net;

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
        }

        private static async Task testScraping()
        {
            Brand brand = new Nike(EBrand.Nike, "https://www.nike.com/kr/launch?s=upcoming");
            await brand.GetNewProduct();
        }
    }
}