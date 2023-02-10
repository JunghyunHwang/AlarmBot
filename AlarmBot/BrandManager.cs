using System.Diagnostics;

namespace AlarmBot
{
    public static class BrandManager
    {
        private static readonly List<Brand> brands = new List<Brand>(8);
        public static bool IsSetBrand { get; private set; } = false;

        static BrandManager()
        {
            SetBrand();
        }

        private static int SetBrand()
        {
            if (IsSetBrand)
            {
                Debug.Assert(false, "Already running program");
                return -1;
            }

            brands.Add(new Nike(EBrand.Nike, "https://www.nike.com/kr/launch?s=upcoming"));
            Debug.Assert(brands.Count == (int)EBrand.Count);

            IsSetBrand = true;

            return 1;
        }

        public static async Task<List<ProductInfo>> CheckNewProducts()
        {
            List<ProductInfo> newProducts = new List<ProductInfo>(64);

            foreach (var b in brands)
            {
                newProducts.AddRange(await b.GetNewProduct(Bot.getProductInfoByBrandName(b.BrandName)));
            }

            return newProducts;
        }
    }
}
