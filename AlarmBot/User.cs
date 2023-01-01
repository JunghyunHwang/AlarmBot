using System;

namespace AlarmBot
{
	public class User
	{
		public EMessenger Messenger { get; private set; }
		public string ChatID { get; private set; }

        public User(EMessenger messenger, string chatID)
        {
            Messenger = messenger;
            ChatID = chatID;
        }
    }
}

