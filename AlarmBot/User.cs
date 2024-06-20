namespace AlarmBot
{
	public class User
	{
        private readonly Messenger mMessenger;
		public string ChatID { get; private set; } // I think that we need to messenger info class

        public User(Messenger messenger, string chatID)
        {
            mMessenger = messenger;
            ChatID = chatID;
        }

        public void SendMessage(ProductInfo p)
        {
            mMessenger.SendMessage(p, this);
        }
    }
}
