using System.Diagnostics;

namespace AlarmBot
{
    public static class BrandManager
    {
        private static readonly Dictionary<EBrand, Brand> brands = new Dictionary<EBrand, Brand>(8);
        public static bool IsSetBrand { get; private set; } = false;

        static BrandManager()
        {
            setBrand();
        }

        private static int setBrand()
        {
            if (IsSetBrand)
            {
                Debug.Assert(false, "Already running program");
                return -1;
            }

            brands.Add(EBrand.Nike, new Nike(EBrand.Nike, "https://www.nike.com/kr/launch?s=upcoming"));
            Debug.Assert(brands.Count == (int)EBrand.Count);

            foreach (var b in brands.Values)
            {
                b.LoadProductByDB();
            }

            IsSetBrand = true;

            return 1;
        }

        public static async Task<List<ProductInfo>> CheckNewProducts()
        {
            List<ProductInfo> newProducts = new List<ProductInfo>(64);

            foreach (var b in brands.Values)
            {
                newProducts.AddRange(await b.GetNewProduct());
            }

            return newProducts;
        }

        public static void RemoveProduct(ProductInfo product)
        {
            brands[product.BrandName].RemoveProduct(product);
        }
    }
}
