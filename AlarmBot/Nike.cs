using HtmlAgilityPack;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace AlarmBot
{
    public sealed class Nike : Brand
    {
        private readonly HttpClient client;
        public static readonly string BaseUrl = "http://www.nike.com";

        public Nike(EBrand brand, string url)
            : base(brand, url)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", "Chrome/109.0.0.0");
        }

        public override void RemoveProduct(ProductInfo product)
        {
            products.Remove(product);
        }

        public override async Task<List<ProductInfo>> GetNewProduct()
        {
            var response = await client.GetAsync(Url);
            string content = await response.Content.ReadAsStringAsync();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            var list = doc.DocumentNode.SelectNodes("//a[@class=\"card-link d-sm-b\"]");

            Debug.Assert(list.Count % 2 == 0);

            List<ProductInfo> newProducts = new List<ProductInfo>(32);
            StringBuilder urlBuilder = new StringBuilder(256);

            for (int i = 0; i < list.Count / 2; ++i)
            {
                int productLinkIndex = list[i].Attributes.Count - 1;

                var res = await client.GetAsync(list[i].Attributes[productLinkIndex].Value);
                string itemHTML = await res.Content.ReadAsStringAsync();

                HtmlDocument itemDoc = new HtmlDocument();
                itemDoc.LoadHtml(itemHTML);

                bool bIsDrawProduct = itemDoc.DocumentNode.SelectNodes("//br").Count == 4 ? true : false;

                if (bIsDrawProduct)
                {
                    urlBuilder.Clear();
                    urlBuilder.Append(BaseUrl).Append(list[i].Attributes[productLinkIndex].Value);

                    uint urlHash = urlFNVHash(urlBuilder.ToString());
                    bool bIsNewProduct = true;

                    foreach (var p in products)
                    {
                        if (urlHash == p.UrlHash && urlBuilder.ToString() == p.Url)
                        {
                            bIsNewProduct = false;
                            break;
                        }
                    }

                    if (!bIsNewProduct)
                    {
                        continue;
                    }

                    newProducts.Add(makeProductInfoByHTML(itemDoc, urlBuilder.ToString(), urlHash));
                }
            }

            products.AddRange(newProducts);
            return newProducts;
        }

        protected override ProductInfo makeProductInfoByHTML(HtmlDocument itemDoc, string url, uint urlHash)
        {
            var tagTypeName = itemDoc.DocumentNode.SelectSingleNode("//h1[@class=\"headline-5 pb3-sm\"]");
            var tagSneakersName = itemDoc.DocumentNode.SelectSingleNode("//h5[@class=\"headline-1 pb3-sm\"]");
            var tagPrice = itemDoc.DocumentNode.SelectSingleNode("//div[@class=\"headline-5 pb6-sm fs14-sm fs16-md\"]");
            var tagDate = itemDoc.DocumentNode.SelectSingleNode("//div[@class=\"available-date-component\"]");
            var meta = itemDoc.DocumentNode.SelectNodes("//meta");
            string imgUrl = "";

            for (int i = 0; i < meta.Count; ++i)
            {
                if (meta[i].Attributes[1].DeEntitizeValue == "og:image")
                {
                    imgUrl = meta[i].Attributes[2].DeEntitizeValue;
                }
            }

            Regex rxPrice = new Regex("\\d+");
            Regex rxDateTime = new Regex("(\\d{1,2})");

            MatchCollection tempPrice = rxPrice.Matches(tagPrice.InnerText);
            MatchCollection tempDateTime = rxDateTime.Matches(tagDate.InnerText);

            string strPrice = tempPrice[0].Value + tempPrice[1].Value;
            uint price = uint.Parse(strPrice);

            int month = int.Parse(tempDateTime[0].Value);
            int day = int.Parse(tempDateTime[1].Value);
            int hours = int.Parse(tempDateTime[2].Value) + 9;
            int minutes = int.Parse(tempDateTime[3].Value);

            DateTime dateTime = new DateTime(DateTime.Now.Year, month, day, hours, minutes, 0);
            DateOnly drawDate = DateOnly.FromDateTime(dateTime);

            return new ProductInfo(BrandName, tagTypeName.InnerText, tagSneakersName.InnerText, price, url, urlHash, drawDate, dateTime, imgUrl);
        }
    }
}
