using HtmlAgilityPack;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace AlarmBot
{
    public sealed class Nike : Brand
    {
        public static readonly string BASE_URL = "https://www.nike.com/kr/";
        private readonly HttpClient mClient;

        public Nike(EBrand brand, string url)
            : base(brand, url)
        {
            mClient = new HttpClient();
            mClient.BaseAddress = new Uri(BASE_URL);
            mClient.DefaultRequestHeaders.Accept.Clear();
            mClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            mClient.DefaultRequestHeaders.Add("User-Agent", "Chrome/109.0.0.0");
        }

        public override async Task<List<ProductInfo>> GetNewProduct()
         {
            HttpResponseMessage response = await mClient.GetAsync(URL);
            string content = await response.Content.ReadAsStringAsync();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            HtmlNodeCollection list = doc.DocumentNode.SelectNodes("//a[@class=\"card-link d-sm-b\"]");

            Debug.Assert(list.Count % 2 == 0);

            List<ProductInfo> newProducts = new List<ProductInfo>(32);
            StringBuilder urlBuilder = new StringBuilder(256);

            for (int i = 0; i < list.Count / 2; ++i)
            {
                int productLinkIndex = list[i].Attributes.Count - 1;

                HttpResponseMessage res = await mClient.GetAsync(list[i].Attributes[productLinkIndex].Value);
                string itemHTML = await res.Content.ReadAsStringAsync();

                HtmlDocument itemDoc = new HtmlDocument();
                itemDoc.LoadHtml(itemHTML);

                if (itemDoc.DocumentNode.SelectNodes("//br").Count != 4)
                {
                    // This is not draw product
                    continue;
                }

                urlBuilder.Clear();
                urlBuilder.Append(BASE_URL).Append(list[i].Attributes[productLinkIndex].Value);

                if (searchProducts.ContainsKey(urlBuilder.ToString()))
                {
                    continue;
                }

                ProductInfo p = makeProductInfoByHTML(itemDoc, urlBuilder.ToString());

                addProduct(p);
                newProducts.Add(p);
            }

            return newProducts;
        }

        protected override ProductInfo makeProductInfoByHTML(HtmlDocument itemDoc, string url)
        {
            HtmlNode tagTypeName = itemDoc.DocumentNode.SelectSingleNode("//h1[@class=\"headline-5 pb3-sm\"]");
            HtmlNode tagSneakersName = itemDoc.DocumentNode.SelectSingleNode("//h5[@class=\"headline-1 pb3-sm\"]");
            HtmlNode tagPrice = itemDoc.DocumentNode.SelectSingleNode("//div[@class=\"headline-5 pb6-sm fs14-sm fs16-md\"]");
            HtmlNode tagDate = itemDoc.DocumentNode.SelectSingleNode("//div[@class=\"available-date-component\"]");
            HtmlNodeCollection meta = itemDoc.DocumentNode.SelectNodes("//meta");
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
            int price = int.Parse(strPrice);

            int month = int.Parse(tempDateTime[0].Value);
            int day = int.Parse(tempDateTime[1].Value);
            int hours = int.Parse(tempDateTime[2].Value) + 9;
            int minutes = int.Parse(tempDateTime[3].Value);

            DateTime dateTime = new DateTime(DateTime.Now.Year, month, day, hours, minutes, 0);
            DateOnly drawDate = DateOnly.FromDateTime(dateTime);

            return new ProductInfo(BrandName, tagTypeName.InnerText, tagSneakersName.InnerText, price, url, drawDate, dateTime, imgUrl);
        }
    }
}
