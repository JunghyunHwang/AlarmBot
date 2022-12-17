using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlarmBot
{
    public static class BrandManager
    {
        private static readonly List<Brand> Brands = new List<Brand>(8);
        
        public static void AddBrand(Brand brand)
        {
            Brands.Add(brand);
        }

        public static void LoadData()
        {
            foreach (var brand in Brands)
            {
                brand.LoadProductsFromDatabase();
            }
        }

        public static void CheckNewProduct()
        {
            foreach (var brand in Brands)
            {
                brand.GetNewProduct();
            }
        }
    }
}
