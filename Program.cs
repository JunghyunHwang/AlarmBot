using System.Data;
using System.Diagnostics;
using System.Net;

namespace AlarmBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Brand nike = new Nike(EBrand.Nike, "https://www.nike.com/kr/launch?s=upcoming");
            //BrandManager.AddBrand(nike);

            //TestCheckNewDrawSpecificTime();

            string brand = "Nike";
            EBrand nike;
            Debug.Assert(Enum.TryParse(brand, out nike));

            Console.WriteLine(nike.ToString());

            Console.ReadLine();
        }

        private static void TestCheckNewDrawSpecificTime()
        {
            
        }
    }
}