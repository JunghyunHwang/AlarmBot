using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlarmBot
{
    public sealed class ProductInfo
    {
        public readonly EBrand BrandName;
        public readonly string TypeName;
        public readonly string ProductName;
        public readonly uint Price;
        public readonly string Url;
        public readonly uint UrlHash;
        public readonly DateTime StartTime;
        public readonly string ImgUrl;

        public ProductInfo(EBrand brandName, string typeName, string productName, uint price, string url, uint urlHash, DateTime startTime, string imgUrl)
        {
            BrandName = brandName;
            TypeName = typeName;
            ProductName = productName;
            Price = price;
            Url = url;
            UrlHash = urlHash;
            StartTime = startTime;
            ImgUrl = imgUrl;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType() || GetHashCode() != obj.GetHashCode())
            {
                return false;
            }

            ProductInfo other = (ProductInfo)obj;

            return (BrandName == other.BrandName
                && TypeName == other.TypeName
                && ProductName == other.ProductName
                && Price == other.Price
                && Url == other.Url
                && UrlHash == other.UrlHash
                && StartTime == other.StartTime
                && ImgUrl == other.ImgUrl);
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash *= 31 + BrandName.GetHashCode();
            hash *= 31 + TypeName.GetHashCode();
            hash *= 31 + ProductName.GetHashCode();
            hash *= 31 + (int)Price;
            hash *= 31 + UrlHash.GetHashCode();
            hash *= 31 + (int)UrlHash;
            hash *= 31 + StartTime.GetHashCode();
            hash *= 31 + ImgUrl.GetHashCode();

            return hash;
        }
    }
}
