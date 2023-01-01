using System;

namespace AlarmBot
{
	public class User
	{
		public EMessenger Messenger { get; private set; }
		public int ChatID { get; private set; }

        public User(EMessenger messenger, int chatID)
        {
            Messenger = messenger;
            ChatID = chatID;
        }
    }
}

