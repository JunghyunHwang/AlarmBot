using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AlarmBot
{
    public static class MessengerManager
    {
        static private readonly Dictionary<EMessenger, Messenger> MESSENGERS = new Dictionary<EMessenger, Messenger>((int)EMessenger.Count);
        static public bool IsSetMessengers { get; private set; } = false;

        static MessengerManager()
        {
            if (IsSetMessengers)
            {
                Debug.Assert(false, "Already set Messengers");
                return;
            }

            MESSENGERS.Add(EMessenger.Telegram, new Telegram());
            Debug.Assert(MESSENGERS.Count == (int)EMessenger.Count);
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
