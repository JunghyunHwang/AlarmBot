namespace AlarmBot
{
    public sealed class ProductInfo
    {
        public readonly EBrand BrandName;
        public readonly string TypeName;
        public readonly string ProductName;
        public readonly int Price;
        public readonly string Url;
        public readonly DateOnly DrawDate;
        public readonly DateTime StartTime;
        public readonly string ImgUrl;

        private readonly List<User> users;

        public ProductInfo(EBrand brandName, string typeName, string productName, int price, string url, DateOnly drawDate, DateTime startTime, string imgUrl)
        {
            BrandName = brandName;
            TypeName = typeName;
            ProductName = productName;
            Price = price;
            Url = url;
            DrawDate = drawDate;
            StartTime = startTime;
            ImgUrl = imgUrl;
            users = new List<User>(DB.userCount);
        }

        public void SendMessage()
        {
            foreach (User u in users)
            {
                u.SendMessage(this);
            }
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
                && DrawDate == other.DrawDate
                && StartTime == other.StartTime
                && ImgUrl == other.ImgUrl);
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash *= 31 + BrandName.GetHashCode();
            hash *= 31 + TypeName.GetHashCode();
            hash *= 31 + ProductName.GetHashCode();
            hash *= 31 + Price;
            hash *= 31 + DrawDate.GetHashCode();
            hash *= 31 + StartTime.GetHashCode();
            hash *= 31 + ImgUrl.GetHashCode();

            return hash;
        }
    }
}
