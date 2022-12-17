using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlarmBot
{
    public class ProductInfo
    {
        public readonly EBrand BrandName;
        public readonly string TypeName;
        public readonly string SneakersName;
        public readonly uint Price;
        public readonly string Url;
        public readonly DateTime StartTime;
        public readonly DateTime EndTime;
        public readonly string ImgUrl;

        public ProductInfo(EBrand brandName, string typeName, string sneakersName, uint price, string url, DateTime startTime, DateTime endTime, string imgUrl)
        {
            BrandName = brandName;
            TypeName = typeName;
            SneakersName = sneakersName;
            Price = price;
            Url = url;
            StartTime = startTime;
            EndTime = endTime;
            ImgUrl = imgUrl;
        }

        public bool Equals(ProductInfo product)
        {
            return (Url == product.Url);
        }
    }
}
