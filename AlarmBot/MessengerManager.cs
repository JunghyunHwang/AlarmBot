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
            setMessengers();
        }

        private static bool setMessengers()
        {
            if (IsSetMessengers)
            {
                Debug.Assert(false, "Already set Messengers");
                return false;
            }

            MESSENGERS.Add(EMessenger.Telegram, new Telegram());
            Debug.Assert(MESSENGERS.Count == (int)EMessenger.Count);

            return true;
        }

        public static void SetNotification(List<ProductInfo> todayDrawProduct)
        {
            foreach (var m in MESSENGERS.Values)
            {
                m.SetNotification(todayDrawProduct);
            }
        }
    }
}
