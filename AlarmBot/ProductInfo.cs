using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlarmBot
{
    public sealed class ProductInfo
    {
        public readonly EBrand BrandName;
        public readonly string Url;
        public readonly DateTime Date;

        public ProductInfo(EBrand brandName, string url, DateTime date)
        {
            BrandName = brandName;
            Url = url;
            Date = date;
        }

        public bool Equals(ProductInfo product)
        {
            return (Url == product.Url);
        }
    }
}
