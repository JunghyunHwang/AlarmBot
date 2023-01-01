using System;
using System.Text;
using System.Net;
using System.Configuration;

namespace AlarmBot
{
	public sealed class TelegramBot : Bot
	{
        public static readonly string BOT_TOKEN = ConfigurationManager.ConnectionStrings["TelegramBotToken"].ConnectionString;
        private static readonly string API_URL = $"https://api.telegram.org/bot{BOT_TOKEN}";

        public override void SetNotification(List<ProductInfo> drawProducts)
        {
            
        }

        public override void SendMessageToAllUsers(ProductInfo product)
        {
            StringBuilder apiBuilder = new StringBuilder(256);
            List<User> users = DB.GetUsersByMessenger(EMessenger.Telegram);

            foreach(var u in users)
            {
                apiBuilder.Clear();

                apiBuilder.Append(API_URL)
                    .Append("/sendPhoto")
                    .Append($"?chat_id={u.ChatID}")
                    .Append($"&photo={product.ImgUrl}")
                    .Append("&reply_markup={\"inline_keyboard\":[[{ \"text\": \"지금 응모하기\", \"url\":")
                    .Append($"\"{product.Url}\"")
                    .Append("}]]}");

            }
        }
    }
}

// { "inline_keyboard": [[{ "text": "지금 응모하기", "url": "url" }]]}