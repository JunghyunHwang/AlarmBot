using System;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.Timers;
using System.Configuration;

namespace AlarmBot
{
	public sealed class TelegramBot : Bot
	{
        public static readonly string BOT_TOKEN = ConfigurationManager.AppSettings["TelegramBotToken"];
        private static readonly string API_URL = $"https://api.telegram.org/bot{BOT_TOKEN}";
        private static readonly HttpClient client = new HttpClient();
        private List<System.Timers.Timer> DrawNotificationTimers = new List<System.Timers.Timer>(8);

        public override void SetNotification(List<ProductInfo> drawProducts)
        {
            DrawNotificationTimers.Clear();

            foreach (var product in drawProducts)
            {
                System.Timers.Timer drawTimer = new System.Timers.Timer();
                
                Debug.Assert(product.StartTime > DateTime.Now);
                double remainingTime = (product.StartTime - DateTime.Now).TotalMilliseconds;

                drawTimer.Interval = remainingTime;
                drawTimer.Elapsed += (sender, e) => SendMessageToAllUsers(product);
                drawTimer.Start();

                DrawNotificationTimers.Add(drawTimer);
            }
        }

        protected override async void SendMessageToAllUsers(ProductInfo product)
        {
            StringBuilder uriBuilder = new StringBuilder(256);
            List<User> users = DB.GetUsersByMessenger(EMessenger.Telegram);

            foreach(var u in users)
            {
                uriBuilder.Clear();

                uriBuilder.Append(API_URL)
                    .Append("/sendPhoto")
                    .Append($"?chat_id={u.ChatID}")
                    .Append($"&photo={product.ImgUrl}")
                    .Append("&reply_markup={\"inline_keyboard\":[[{ \"text\": \"지금 응모하기\", \"url\":")
                    .Append($"\"{product.Url}\"")
                    .Append("}]]}");

                await client.GetAsync(uriBuilder.ToString());
            }
        }
    }
}
