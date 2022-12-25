﻿using System;
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

        public static async Task GetNewProduct()
        {
            List<ProductInfo> newProducts = new List<ProductInfo>(64);

            foreach (var b in brands)
            {
                newProducts.AddRange(await b.GetNewProduct());
            }

            DB.InsertProducts(newProducts);
        }
    }
}