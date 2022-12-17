using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlarmBot
{
    public abstract class Brand
    {
        public readonly EBrand BrandName;
        public readonly string Url;
        protected List<ProductInfo> upcommingProducts;

        public Brand(EBrand brandName, string url)
        {
            BrandName = brandName; 
            Url = url;
            upcommingProducts = new List<ProductInfo>(32);
        }

        public abstract int GetNewProduct();

        public void LoadProductsFromDatabase()
        {

        }
    }
}
