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

        private static void TestSendMessage()
        {
            BrandManager.SetBrand();
            DrawTimer.StartTimer();
        }
    }
}