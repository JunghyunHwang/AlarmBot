using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlarmBot
{
    public sealed class ProductInfo
    {
        public readonly uint ID;
        public readonly EBrand BrandName;
        public readonly string TypeName;
        public readonly string ProductName;
        public readonly uint Price;
        public readonly string Url;
        public readonly uint UrlHash;
        public readonly DateTime StartTime;
        public readonly string ImgUrl;

        public ProductInfo(uint id, EBrand brandName, string typeName, string productName, uint price, string url, uint urlHash, DateTime startTime, string imgUrl)
        {
            ID = id;
            BrandName = brandName;
            TypeName = typeName;
            ProductName = productName;
            Price = price;
            Url = url;
            UrlHash = urlHash;
            StartTime = startTime;
            ImgUrl = imgUrl;
        }

        public bool Equals(ProductInfo product)
        {
            return (UrlHash == product.UrlHash);
        }
    }
}
