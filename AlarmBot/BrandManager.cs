using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlarmBot
{
    public static class BrandManager
    {
        private static readonly List<Brand> brands = new List<Brand>(8);
        
        public static void AddBrand(Brand brand)
        {
            brands.Add(brand);
        }

        public static void LoadData()
        {
            foreach (var b in brands)
            {
                b.LoadProductsFromDatabase();
            }
        }

        public static async Task CheckNewProduct()
        {
            foreach (var b in brands)
            {
                await b.CheckNewProduct();
            }
        }
    }
}
