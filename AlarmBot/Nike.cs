using HtmlAgilityPack;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Crmf;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            client.DefaultRequestHeaders.Add("User-Agent", "Chrome/108.0.5359.124");
        }

        public override async Task<List<ProductInfo>> GetNewProduct(List<ProductInfo> existingProducts)
        {
            var response = await client.GetAsync(Url);
            string content = await response.Content.ReadAsStringAsync();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            var list = doc.DocumentNode.SelectNodes("//a[@class=\"card-link d-sm-b\"]");

            Debug.Assert(list.Count % 2 == 0);

            List<ProductInfo> newProduct = new List<ProductInfo>(32);
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

                    foreach (var p in existingProducts)
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

                    var newP = makeProductInfoByHTML(itemDoc, urlBuilder.ToString(), urlHash);
                    Debug.Assert(newP != null);

                    newProduct.Add(newP);
                }
            }

            return newProduct;
        }

        protected override ProductInfo makeProductInfoByHTML(HtmlDocument itemDoc, string url, uint urlHash)
        {
            var tagTypeName = itemDoc.DocumentNode.SelectSingleNode("//h1[@class=\"headline-5 pb3-sm\"]");
            var tagSneakersName = itemDoc.DocumentNode.SelectSingleNode("//h5[@class=\"headline-1 pb3-sm\"]");
            var tagPrice = itemDoc.DocumentNode.SelectSingleNode("//div[@class=\"headline-5 pb6-sm fs14-sm fs16-md\"]");
            var tagDate = itemDoc.DocumentNode.SelectSingleNode("//div[@class=\"available-date-component\"]");
            var imgUrl = itemDoc.DocumentNode.SelectSingleNode("//div[@class=\"image-component\"]");

            Regex rxPrice = new Regex("\\d+");
            Regex rxDateTime = new Regex("(\\d{1,2})");

            MatchCollection tempPrice = rxPrice.Matches(tagPrice.InnerText);
            MatchCollection tempDateTime = rxDateTime.Matches(tagDate.InnerText);

            string strPrice = tempPrice[0].Value + tempPrice[1].Value;
            uint price = uint.Parse(strPrice);

            int month = int.Parse(tempDateTime[0].Value);
            int day = int.Parse(tempDateTime[1].Value);
            int hours = int.Parse(tempDateTime[2].Value);
            int minutes = int.Parse(tempDateTime[3].Value);

            DateTime dateTime = new DateTime(DateTime.Now.Year, month, day, hours, minutes, 0);

            return new ProductInfo(BrandName, tagTypeName.InnerText, tagSneakersName.InnerText, price, url, urlHash, dateTime, imgUrl.InnerText);
        }
    }
}
