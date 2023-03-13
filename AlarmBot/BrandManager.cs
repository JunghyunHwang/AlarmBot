using System.Diagnostics;

namespace AlarmBot
{
    public static class BrandManager
    {
        private static readonly Dictionary<EBrand, Brand> BRANDS = new Dictionary<EBrand, Brand>((int)EBrand.Count);
        public static bool IsSetBrands { get; private set; } = false;

        static BrandManager()
        {
            setBrands();
        }

        private static bool setBrands()
        {
            if (IsSetBrands)
            {
                Debug.Assert(false, "Already running program");
                return false;
            }

            BRANDS.Add(EBrand.Nike, new Nike(EBrand.Nike, "https://www.nike.com/kr/launch?s=upcoming"));
            Debug.Assert(BRANDS.Count == (int)EBrand.Count);

            foreach (var b in BRANDS.Values)
            {
                b.LoadProductByDB();
            }

            IsSetBrands = true;

            return true;
        }

        public static async Task<List<ProductInfo>> CheckNewProducts()
        {
            List<ProductInfo> newProducts = new List<ProductInfo>(64);

            foreach (var b in BRANDS.Values)
            {
                newProducts.AddRange(await b.GetNewProduct());
            }

            return newProducts;
        }

        public static void RemoveProduct(ProductInfo product)
        {
            BRANDS[product.BrandName].RemoveProduct(product);
        }
    }
}
