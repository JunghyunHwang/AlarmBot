using System.Diagnostics;

namespace AlarmBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ProductInfo product1 = new ProductInfo(EBrand.Nike, "airforce", "test", 200, "naver.com", DateTime.Now, DateTime.Now, "google.com");
            ProductInfo product2 = new ProductInfo(EBrand.Nike, "airforce", "test", 200, "naver.com", DateTime.Now, DateTime.Now, "google.com");
            ProductInfo product3 = new ProductInfo(EBrand.Nike, "airforce", "test", 200, "naver.com", DateTime.Now, DateTime.Now, "google.com");

            Debug.Assert(product1 == product2);
            Debug.Assert(product2 != product3);

            Brand nike = new Nike(EBrand.Nike, "www.nike.com");
            Brand adidas = new Nike(EBrand.Adidas, "www.naver.com");

            BrandManager.AddBrand(nike);
            BrandManager.AddBrand(adidas);

            Console.WriteLine("No prob");
        }
    }
}