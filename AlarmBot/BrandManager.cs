using System.Diagnostics;

namespace AlarmBot
{
    public static class BrandManager
    {
        static private readonly Dictionary<EBrand, Brand> BRANDS = new Dictionary<EBrand, Brand>((int)EBrand.Count);
        static public bool IsSetBrands { get; private set; } = false;

        static BrandManager()
        {
            if (IsSetBrands)
            {
                Debug.Assert(false, "Already running program");
                return;
            }

            // Set brands
            BRANDS.Add(EBrand.Nike, new Nike(EBrand.Nike, "https://www.nike.com/kr/launch?s=upcoming"));
            Debug.Assert(BRANDS.Count == (int)EBrand.Count);

            foreach (Brand b in BRANDS.Values)
            {
                b.LoadProductByDB();
            }

            IsSetBrands = true;
        }

        public static async Task<List<ProductInfo>> CheckNewProducts()
        {
            List<ProductInfo> newProducts = new List<ProductInfo>(64);

            foreach (Brand b in BRANDS.Values)
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
