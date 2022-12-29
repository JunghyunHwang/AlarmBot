using System.Data;
using System.Diagnostics;
using System.Net;

namespace AlarmBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Brand nike = new Nike(EBrand.Nike, "https://www.nike.com/kr/launch?s=upcoming");
            BrandManager.AddBrand(nike);

            TestCheckNewDrawSpecificTime();

            Console.ReadLine();
        }

        private static void TestCheckNewDrawSpecificTime()
        {
            DrawTimer.SetTimerElapsedEventHandler();
            DrawTimer.StartCheckDrawTimer();
        }
    }
}