using System.Diagnostics;
using System.Text;
using System.Configuration;

namespace AlarmBot
{
	public sealed class Telegram : Messenger
	{
#pragma warning disable CS8601 // Possible null reference assignment.
        private static readonly string BOT_TOKEN = ConfigurationManager.AppSettings["TelegramBotToken"];
#pragma warning restore CS8601 // Possible null reference assignment.
        private static readonly string API_URL = $"https://api.telegram.org/bot{BOT_TOKEN}";

        private readonly HttpClient mClient = new HttpClient();
        private readonly StringBuilder mUriBuilder = new StringBuilder(256);

        public override void SendMessage(ProductInfo drawProduct, User user)
        {
            mUriBuilder.Clear();

            mUriBuilder.Append(API_URL)
                    .Append("/sendPhoto")
                    .Append($"?chat_id={user.ChatID}")
                    .Append($"&photo={drawProduct.ImgUrl}")
                    .Append("&reply_markup={\"inline_keyboard\":[[{ \"text\": \"지금 응모하기\", \"url\":")
                    .Append($"\"{drawProduct.Url}\"")
                    .Append("}]]}");

            mClient.GetAsync(mUriBuilder.ToString());
        }
    }
}
