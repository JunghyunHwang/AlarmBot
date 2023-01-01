using System.Data;
using System.Diagnostics;
using System.Net;

namespace AlarmBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TestSendMessage();

            Console.ReadLine();
        }

        private static uint urlFNVHash(string url)
        {
            const uint FNV_PRIME_32 = 16777619;
            uint hash = 2166136261U;

            for (int i = 0; i < url.Length; ++i)
            {
                hash *= FNV_PRIME_32;
                hash ^= url[i];
            }

            return hash;
        }

        private static void TestSendMessage()
        {
            BrandManager.SetBrand();
            DrawTimer.StartTimer();
        }
    }
}