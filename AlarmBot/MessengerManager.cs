using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AlarmBot
{
    public static class MessengerManager
    {
        private static readonly Dictionary<EMessenger, Messenger> MESSENGERS = new Dictionary<EMessenger, Messenger>((int)EMessenger.Count);
        public static bool IsSetMessengers { get; private set; } = false;

        static MessengerManager()
        {
            MESSENGERS.Add(EMessenger.Telegram, new Telegram());
            Debug.Assert(MESSENGERS.Count == (int)EMessenger.Count);

            IsSetMessengers = true;
        }

        public static void SetNotification(List<ProductInfo> todayDrawProduct)
        {
            foreach (Messenger m in MESSENGERS.Values)
            {
                m.SetNotification(todayDrawProduct);
            }
        }
    }
}
